using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.WebService.Utils;

namespace FLChat.WebService.Handlers.Auth
{
    /// <summary>
    /// Returns authorization token
    /// </summary>
    public class GetToken : IObjectedHandlerStrategy<TokenRequest, TokenResponse>
    {
        private readonly ITokenRecoverFactory<TokenPayload> _factory;

        public GetToken(ITokenRecoverFactory<TokenPayload> factory) {
            _factory = factory;
        }

        /// <summary>
        /// Expiration time in seconds for token
        /// </summary>
        public int ExpireBy { get; set; } = 60 * 60 * 24 * 90;

        /// <summary>
        /// Possible refresh period in seconds after expiration
        /// </summary>
        public int RefreshPeriod { get; set; } = 60 * 60 * 24 * 90;

        public bool IsReusable => true;

        public TokenResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, TokenRequest input) {
            if (input.Token != null)
                return RefreshToken(entities, input.Token);
            else
                return TokenBySmsCode(entities, input);
        }

        /// <summary>
        /// Refresh token using expired token
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="oldToken">expired token</param>
        /// <returns>new token</returns>
        private TokenResponse RefreshToken(ChatEntities entities, string oldToken) {
            //decode payload
            TokenPayload paylod = null;
            try {
                 paylod = _factory.Decode(oldToken);
            } catch (Exception e) {
                throw new ErrorResponseException(HttpStatusCode.Unauthorized, ErrorResponse.Kind.invalid_auth_token, e);
            }

            //search token in database
            AuthToken dbtoken = entities
                .AuthToken
                .Where(t => t.Token == oldToken)
                .Include(t => t.User)
                .SingleOrDefault();

            if (dbtoken == null)
                throw new ErrorResponseException(HttpStatusCode.Unauthorized, ErrorResponse.Kind.missed_auth_token, "Token not found");
            
            //check token fields, confirm this token has issued by our service
            if (!(paylod.UserId == dbtoken.UserId 
                && Math.Abs((paylod.Iss - dbtoken.IssueDate).TotalMinutes) < 1
                && paylod.Exp == dbtoken.ExpireBy))
                throw new ErrorResponseException(HttpStatusCode.Unauthorized, ErrorResponse.Kind.invalid_auth_token, "Unknown token");

            //if user is disabled or isn't consultant, then can't refresh token
            if (!(dbtoken.User.Enabled && dbtoken.User.IsConsultant))
                throw new ErrorResponseException(HttpStatusCode.Unauthorized, ErrorResponse.Kind.user_not_found, "Can't refresh token for this user");

            //check refresh period
            if ((DateTime.Now - dbtoken.IssueDate).TotalSeconds > dbtoken.ExpireBy + RefreshPeriod)
                throw new ErrorResponseException(HttpStatusCode.Unauthorized, ErrorResponse.Kind.expired, "Token's refresh period are gone");

            return MakeToken(entities, dbtoken);
        }

        /// <summary>
        /// Get new token using sms code
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        private TokenResponse TokenBySmsCode(ChatEntities entities, TokenRequest input) {
            //check input data correct
            if (String.IsNullOrWhiteSpace(input.Phone))
                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, "Phone number can't be empty");

            if (!int.TryParse(input.SmsCode, out int code))
                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, "Sms code is invalid");

            //search smsCode
            SmsCode smsCode = entities
                .SmsCode
                .Where(c => c.User.Phone == input.Phone && c.Code == code)
                .Include(c => c.User)
                .SingleOrDefault();

            if (smsCode != null)
            {
                //if user is disabled or isn't consultant, then can't refresh token
                if (!(smsCode.User.Enabled && smsCode.User.IsConsultant))
                    throw new ErrorResponseException(HttpStatusCode.Unauthorized, ErrorResponse.Kind.user_not_found, "Can't refresh token for this user");

                if (smsCode.IssueDate.AddSeconds(smsCode.ExpireBySec) < DateTime.Now)
                    throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.expired, "Sms code expired");

                Guid userId = smsCode.UserId;

                //save sms code as User's password
                smsCode.User.PswHash = input.SmsCode.ComputeMD5();

                //delete sms core record
                entities.Entry(smsCode).State = System.Data.Entity.EntityState.Deleted;

                return MakeToken(entities, userId);
            } else {
                DAL.Model.User user = SeekUserByPasswordAndPhone(entities, input.Phone, input.SmsCode);
                if (user == null)
                    throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.not_found, "Sms code not found");
                else
                {
                    //if user is disabled or isn't consultant, then can't refresh token
                    if (!(user.Enabled && user.IsConsultant))
                        throw new ErrorResponseException(HttpStatusCode.Unauthorized, ErrorResponse.Kind.user_not_found, "Can't refresh token for this user");

                    return MakeToken(entities, user.Id);
                }
            }            
        }

        /// <summary>
        /// Generates new token, saves token to database and returns response
        /// </summary>
        /// <param name="entities">Database entities</param>
        /// <param name="userId">User id</param>
        /// <returns>Response data</returns>
        private TokenResponse MakeToken(ChatEntities entities, Guid userId) 
        {
            DateTime issueDate = DateTime.Now;
            int expireBy = ExpireBy;

            AuthToken dbToken = new AuthToken() {
                UserId = userId
            };
            entities.AuthToken.Add(dbToken);

            return MakeToken(entities, dbToken);
        }

        /// <summary>
        /// Generates new token, saves token to database and returns response
        /// </summary>
        /// <param name="entities">Database entities</param>
        /// <param name="dbToken">Existed token in database. Can't be null</param>
        /// <returns>Response data</returns>
        private TokenResponse MakeToken(ChatEntities entities, AuthToken dbToken) {
            DateTime issueDate = DateTime.Now;
            int expireBy = ExpireBy;

            dbToken.Token = _factory.Gen(dbToken.UserId, issueDate, expireBy);
            dbToken.ExpireBy = expireBy;
            dbToken.IssueDate = issueDate;
            entities.SaveChanges();

            return new TokenResponse() { Token = dbToken.Token };
        }

        private DAL.Model.User SeekUserByPasswordAndPhone(ChatEntities entities, string phone, string psw) {
            string hash = psw.ComputeMD5();
            return entities.User.Where(u => u.Enabled && u.Phone == phone && u.PswHash == hash).FirstOrDefault();            
        }
    }
}
