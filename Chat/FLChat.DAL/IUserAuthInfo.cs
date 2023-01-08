using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL
{
    /// <summary>
    /// Basic information about user
    /// </summary>
    public interface IUserAuthInfo
    {
        Guid UserId { get; }
    }
}
