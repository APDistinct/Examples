using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService
{
    public interface IPasswordChecker
    {
        void CheckPassword(string psw);
    }

    public class PasswordChecker : IPasswordChecker
    {
        private int minLenth = 6;

        public void CheckPassword(string psw)
        {
            // Непустота
            if (string.IsNullOrEmpty(psw))
                throw new ErrorResponseException(
                      (int)HttpStatusCode.BadRequest,
                      new ErrorResponse(ErrorResponse.Kind.not_support, $"Empty password is not permitted"));
            // проверка длины
            if (psw.Length < minLenth)
                throw new ErrorResponseException(
                      (int)HttpStatusCode.BadRequest,
                      new ErrorResponse(ErrorResponse.Kind.not_support, $"Lenth of password must be grater or equal then {minLenth}.  {psw} not permitted"));
        }
    }
}
