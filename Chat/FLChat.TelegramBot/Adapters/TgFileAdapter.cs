using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;

using FLChat.Core;
using Telegram.Bot.Types;

namespace FLChat.TelegramBot.Adapters
{
    public class TgFileAdapter : Core.IInputFile
    {
        private Core.IInputFile _origin;
        private Telegram.Bot.Types.File _tgFile;

        public TgFileAdapter(Core.IInputFile origin, File tgFile) {
            _origin = origin;
            _tgFile = tgFile;
        }

        public MediaGroupKind Type => _origin.Type;

        public string Media => _origin.Media;

        public string FileName {
            get {
                if (_tgFile.FilePath == null)
                    return _origin.FileName;
                int index = _tgFile.FilePath.LastIndexOf('/');
                if ( index >= 0) {
                    return _tgFile.FilePath.Substring(index + 1);
                } else
                    return _tgFile.FilePath;
            }
        }

        public string MediaType => _origin.MediaType;
    }
}
