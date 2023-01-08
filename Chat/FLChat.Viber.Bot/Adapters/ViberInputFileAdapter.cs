using FLChat.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.Viber.Client.Types;

namespace FLChat.Viber.Bot.Adapters
{
    public class ViberInputFileAdapter : ViberAdapter, IInputFile
    {
        public ViberInputFileAdapter(CallbackData callback) : base(callback) {
            switch (Callback.Message.Type) {
                case MessageType.Picture:
                    Type = MediaGroupKind.Image;
                    break;
                case MessageType.File:
                case MessageType.Video:
                    Type = MediaGroupKind.Document;
                    break;
                default:
                    throw new InvalidOperationException($"Viber message type {Callback.Message.Type.ToString()} are not contains file");
            }
            Media = callback.Message.Media;
            FileName = Callback.Message.FileName;
            if (FileName == null) {
                int index = Media.LastIndexOf('/');
                if (index >= 0) {
                    int index2 = Media.IndexOf('?', index);
                    if (index2 > 0)
                        FileName = Media.Substring(index + 1, index2 - index - 1);
                    else
                        FileName = Media.Substring(index + 1);
                } else
                    FileName = Media;
            }
        }

        public MediaGroupKind Type { get; }

        public string Media { get; }

        public string FileName { get; }

        public string MediaType => null;

        public static bool IsContainsFile(CallbackData data) {
            if (data.Message == null)
                return false;

            switch (data.Message.Type) {
                case MessageType.Picture:
                case MessageType.File:
                case MessageType.Video:
                    return true;
                default:
                    return false;
            }
        }
    }
}
