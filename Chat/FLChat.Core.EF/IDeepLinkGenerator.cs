using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core
{
    public interface IDeepLinkGenerator
    {
        string Generate(User u);
    }
}
