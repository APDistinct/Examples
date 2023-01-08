using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core
{
    public class ActionFileLoader : IFileLoader
    {
        private readonly Func<IInputFile, DownloadFileResult> _func;

        public ActionFileLoader(Func<IInputFile, DownloadFileResult> func) {
            _func = func;
        }

        public DownloadFileResult Download(IInputFile file) {
            return _func(file);
        }
    }
}
