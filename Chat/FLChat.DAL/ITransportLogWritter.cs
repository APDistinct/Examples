using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL
{
    public interface ITransportLogWritter
    {
        long Insert(bool outcome, string url, string method, string request, int statusCode = 0, string response = null);
        void Update(long id, string response);
        void UpdateException(long id, string exception);
    }
}
