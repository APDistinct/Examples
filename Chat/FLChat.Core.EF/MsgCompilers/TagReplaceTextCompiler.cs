using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.Core.MsgCompilers
{
    public class TagReplaceTextCompiler : IMessageTextCompilerWithCheck
    {
        public Dictionary<string, Func<MessageToUser, string>> ReplaceDict { get; }
        public string[] _hashList => ReplaceDict.Keys/*.Select(x => x.Key)*/.ToArray();
        //private Func<MessageToUser, MessageToUser> _func;

        public string Tag => "#";
        private int Step => Tag.Length;
        private bool _force;

        public TagReplaceTextCompiler(Dictionary<string, Func<MessageToUser, string>> replaceDict = null, bool force = false)
        {
            ReplaceDict = replaceDict ?? new Dictionary<string, Func<MessageToUser, string>>();
            _force = force;
        }

        public string MakeText(MessageToUser mtu, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;
            //var mtuu = _func(mtu);
            string result = text ?? "";
            if (_force || NeedToChange(mtu))
            {
                result = Replace(text, mtu);
            }
            return result;
        }


        private string Replace(string str, MessageToUser mtu)
        {
            var result = new StringBuilder();

            int index = 0; // str.IndexOf(Tag);
            int startSeach = 0;
            int startCopy = 0;
            string res;

            while (index >= 0)
            {
                index = str.IndexOf(Tag, startSeach);
                if (index >= 0)
                {
                    startSeach = index + Step;
                    foreach (var rep in ReplaceDict)
                    {

                        if(str.Length - (index + Step) - rep.Key.Length >= 0)
                        if (str.Substring(index + Step, rep.Key.Length) == rep.Key)
                        {
                            result.Append(str,startCopy, index - startCopy);
                            //result.Append(str.Substring(startCopy, index - startCopy));

                            result.Append(rep.Value?.Invoke(mtu) ?? "");
                            startSeach += rep.Key.Length;
                            startCopy = startSeach;
                            break;
                        }
                        
                    }
                }
            }
            if (result.Length > 0)
            {
                result.Append(str.Substring(startCopy, str.Length - startCopy));
                res = result.ToString();
            }
            else
                res = str;
            return res;
        }

        //private string Replace1(string str, MessageToUser mtu)
        //{
        //    var result = new StringBuilder();

        //    var index = str.IndexOf(Tag);
        //    var nextIndex = 0;

        //    if (index > 0)
        //        result.Append(str.Substring(0, index));

        //    while (index >= 0)
        //    {
        //        foreach (var rep in ReplaceDict)
        //        {
        //            if (str.Substring(index + Step, rep.Key.Length) == rep.Key)
        //            {
        //                result.Append(rep.Value(mtu));

        //                var skipCount = index + rep.Key.Length + Step;
        //                nextIndex = str.IndexOf(Tag, skipCount);

        //                int skip = index + rep.Key.Length + Step;
        //                result.Append(nextIndex < 0 //== -1
        //                    ? str.Substring(skip, str.Length - 1)
        //                    : str.Substring(skip, nextIndex - skip));

        //                break;
        //            }
        //        }

        //        index = nextIndex;
        //    }

        //    return result.ToString();
        //}

        public bool IsChangable(string text)
        {
            if (String.IsNullOrEmpty(text))
                return false;
            foreach (var hash in _hashList)
            {
                if (text.Contains(hash))
                    return true;
            }
            return false;
        }

        public static bool NeedToChange(MessageToUser mtu)
        {
            return mtu.Message.NeedToChangeText;
        }
    }
}
