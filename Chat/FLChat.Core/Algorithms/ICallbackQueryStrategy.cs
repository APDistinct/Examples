using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core.Algorithms
{
    public interface ICallbackQueryStrategy<TDb>
    {
        void Process(TDb db, ICallbackData callbackData);
    }
}
