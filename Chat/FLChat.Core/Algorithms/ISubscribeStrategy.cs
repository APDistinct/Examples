using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core.Algorithms
{
    public interface ISubscribeStrategy<TDb>
    {
        void Process(TDb db, ISubscribeData subscribeData);
    }
}
