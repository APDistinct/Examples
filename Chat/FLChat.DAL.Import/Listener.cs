using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Import
{
    public interface IListener
    {
        void Progress(int stage, int index, bool updated);
        void Warning(int index, string text);
        void OnCommit(int index);
    }
}
