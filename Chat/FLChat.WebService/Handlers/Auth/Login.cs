using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.WebService.Handlers.Message;

namespace FLChat.WebService.Handlers.Auth
{
    public class Login : IObjectedHandlerStrategy<LoginRequest, LoginResponse>
    {
        private static Random _rnd = new Random();
        private static object _rndLock = new object();
        private int _maxValue;
        private sbyte _codeDigitsCount;

        public Login() {
            CodeDigitsCount = 6;
        }

        /// <summary>
        /// Minimal interval between two consequtive sms
        /// </summary>
        public int MinIntervalBetweenSms { get; set; } = 60;

        /// <summary>
        /// Count of code's digits
        /// </summary>
        public sbyte CodeDigitsCount {
            get => _codeDigitsCount;
            set {
                _codeDigitsCount = Math.Max((sbyte)3, Math.Min(value, (sbyte)12));
                _maxValue = (int)Math.Pow(10, _codeDigitsCount);
            }
        }

        /// <summary>
        /// Maximum code value
        /// </summary>
        public int CodeMaxValue => _maxValue;

        /// <summary>
        /// Expiration type for new token in seconds
        /// </summary>
        public int ExpireBy { get; set; } = 5 * 60;

        public bool IsReusable => true;

        public LoginResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, LoginRequest input) {
            if (String.IsNullOrWhiteSpace(input.Phone))
                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, "Phone number can't be empty");

            DAL.Model.User user = entities
                .User
                .Where(u => u.Enabled && u.IsConsultant && u.Phone == input.Phone && u.Phone != null)
                .Include(u => u.SmsCode)
                .SingleOrDefault();
            if (user == null)
                throw new ErrorResponseException(HttpStatusCode.Unauthorized, ErrorResponse.Kind.user_not_found, "User with this phone number has not found");

            int smsCode = -1;
            //if exist previous sms
            if (user.SmsCode != null) {
                double interval = (DateTime.Now - user.SmsCode.IssueDate).TotalSeconds;

                //if interval too small
                if (interval < MinIntervalBetweenSms)
                    return new LoginResponse() {
                        Status = LoginResponse.StatusEnum.Waiting,
                        WaitingTime = MinIntervalBetweenSms - (int)interval };

                //if previous sms is not expired, then use previous sms code
                if (interval < user.SmsCode.ExpireBySec)
                    smsCode = user.SmsCode.Code;
            } else {
                user.SmsCode = new SmsCode() {
                    UserId = user.Id
                };
            }

            //generate random number, if need
            if (smsCode == -1) {
                lock (_rndLock) {
                    smsCode = _rnd.Next(_maxValue);
                }
            }

            //send sms
            string textCode = String.Concat("Your FLChat login code: ", smsCode.ToString(new string('0', _codeDigitsCount)));
            SmsBotSend(entities, user, textCode);

            //save sms code
            user.SmsCode.Code = smsCode;
            user.SmsCode.IssueDate = DateTime.Now;
            user.SmsCode.ExpireBySec = ExpireBy;
            entities.SaveChanges();

            //return result
            return new LoginResponse() {
                Status = LoginResponse.StatusEnum.Sent
            };
        }
        private void SmsBotSend(ChatEntities entities, DAL.Model.User user, string textCode)
        {
            DAL.Model.Message msg = new DAL.Model.Message() {
                Kind = MessageKind.Personal,
                FromUserId = Global.SystemBotId,
                FromTransportKind = TransportKind.FLChat,
                Text = textCode,
            };
            msg.ToUsers.Add(new MessageToUser() {
                ToUserId = user.Id,
                ToTransportKind = TransportKind.Sms,
            });
            entities.Message.Add(msg);
        }
    }
}
