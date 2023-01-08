using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.WebService.Utils;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.Handlers.User
{
    public class SetProfilePassword : IObjectedHandlerStrategy<PasswordRequest, object>
    {
        //public const string KeyFieldName = "password";
        public bool IsReusable => true;
        IPasswordChecker _passwordChecker;

        public SetProfilePassword(IPasswordChecker pswChecker = null)
        {
            _passwordChecker = pswChecker ?? new PasswordChecker();
        }

        public object ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, PasswordRequest input)
        {
            //save code as User's password
            string passw = input.Password;
            DAL.Model.User user = entities.User
               .Where(u => u.Id == currUserInfo.UserId )               
               .SingleOrDefault();
            if (user == null)
                throw new ErrorResponseException(HttpStatusCode.NotFound, ErrorResponse.Kind.user_not_found, 
                    $"User with id {currUserInfo.UserId} has not found");
            if (user.PswHash != null)
            {
                if(user.PswHash != input.OldPassword?.ComputeMD5())
                    throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.wrong_old_password, 
                        $"Bad user's old password <{input.OldPassword ?? ""}>");
            }
            _passwordChecker.CheckPassword(passw);
            user.PswHash = passw.ComputeMD5();
            entities.SaveChanges();
            return null;
        }
    }
}
