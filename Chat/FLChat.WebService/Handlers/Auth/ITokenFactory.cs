using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.Handlers.Auth
{
    public interface ITokenFactory
    {
        string Gen(Guid id, DateTime issueDate, int expireBy);
    }

    public interface ITokenRecoverFactory<T> : ITokenFactory
    {
        /// <summary>
        /// Decode token
        /// </summary>
        /// <param name="token">Token string</param>
        /// <returns>Returns token information, can't be null</returns>
        T Decode(string token);
    }

    public class InvalidTokenException : ApplicationException
    {
        public InvalidTokenException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
