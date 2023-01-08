using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.Handlers.Message
{   
    public interface IMessageEditTimeChecker
    {
        bool Delay(DateTime dt);        
    }

    public class MessageEditTimeChecker : IMessageEditTimeChecker
    {
        private readonly int _delay = 1000;
        //private readonly string _pattern;

        public MessageEditTimeChecker(string pattern = null)
        {
            if(pattern != null)
            {
                // "MESS_DELAY_TIMEOUT"
                var str = Settings.Values.GetValue(pattern, "1000");
                if(int.TryParse(str,out int res))
                    _delay = res;  //  добывать из конфига по параметру
            }            
        }

        public bool Delay(DateTime dt)
        {
            return DateTime.UtcNow.AddMilliseconds(_delay) < dt;
        }        
    }
}
