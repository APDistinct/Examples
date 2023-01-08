using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;
using FLChat.Core.MsgCompilers;

namespace FLChat.Core
{
    public interface IMessageTextCompiler
    {
        string MakeText(MessageToUser mtu, string text);     
    }
    public interface IMessageBulkTextCompiler
    {        
        string MakeText(Message mtu, string text);
    }

    public interface IMessageTextCompilerWithCheck : IMessageTextCompiler
    {
        /// <summary>
        /// check is <paramref name="text"/> has any tags
        /// </summary>
        /// <param name="text">string with tags for replace</param>
        /// <returns>true if <paramref name="text"/> has tags</returns>
        bool IsChangable(string text);
    }

    public static class IMessageTextCompilerExtentions
    {
        public static string MakeText(this IMessageTextCompiler compiller, MessageToUser mtu)
        {
            return compiller.MakeText(mtu, mtu.Message.Text);
        }

        public static string MakeText(this IMessageBulkTextCompiler compiller, Message mtu)
        {
            return compiller.MakeText(mtu, mtu.Text);
        }

        public static string MakeText(this IMessageTextCompiler compiler, string text, User u) {
            return compiler.MakeText(new MessageToUser() {
                Message = new Message() {
                    Text = text,
                    NeedToChangeText = true
                },
                ToTransport = new Transport {
                    User = u
                },
            });
        }

        public static IMessageTextCompiler Add(this IMessageTextCompiler compiler, IMessageTextCompiler addCompiller)
        {
            List<IMessageTextCompiler> list = new List<IMessageTextCompiler> {compiler};
            if (addCompiller != null)
                list.Add(addCompiller);
            return new ChainCompiler(list);
        }

        /// <summary>
        /// Returns ChainCompiler with <paramref name="compiler"/> and instance of HashReplaceTextCompiler 
        /// </summary>
        /// <param name="compiler">compiter for unite</param>
        /// <returns>ChainCompiler</returns>
        public static IMessageTextCompiler UniteWithHashCompiler(this IMessageTextCompiler compiler, bool html = false) {
            List<IMessageTextCompiler> list = new List<IMessageTextCompiler> {
                CreateTagTextCompiler(html)
            };
            if (compiler != null)
                list.Add(compiler);
            return new ChainCompiler(list);
        }

        /// <summary>
        /// Returns ChainCompiler with <paramref name="compiler"/> and instance of HashReplaceTextCompiler 
        /// </summary>
        /// <param name="compiler">compiter for unite</param>
        /// <returns>ChainCompiler</returns>
        public static IMessageTextCompiler AddStandartHashCompiler(this IMessageTextCompiler compiler, bool html = false)
        {
            List<IMessageTextCompiler> list = new List<IMessageTextCompiler> {
                CreateTagTextCompiler(html)
            };
            if (compiler != null)
                list.Insert(0, compiler); // list.Add(compiler);
            return new ChainCompiler(list);
        }

        /// <summary>
        /// Create TagReplaceTextCompiler with default tags
        /// </summary>
        /// <param name="html">generate url with html tags</param>
        /// <param name="genLink">replace link tag with generated url</param>
        /// <returns>TagReplaceTextCompiler</returns>
        public static IMessageTextCompilerWithCheck CreateTagTextCompiler(bool html = false, bool genLink = true)
        {
            IDeepLinkGenerator gen = new Algorithms.LiteDeepLinkStrategy();
            Dictionary<string, Func<MessageToUser, string>> ReplaceDict
                = new Dictionary<string, Func<MessageToUser, string>>()
                {
                    { "ФИО", mtu => mtu.ToTransport.User.FullName ?? "" },
                    { "город", mtu => mtu.ToTransport.User.City?.Name ?? "" },
                    { "OwnerUser" , mtu => mtu.ToTransport.User.OwnerUser?.FullName ?? "" },
                };
            if (genLink)
            {
                ReplaceDict["ссылка"] = mtu => {
                    string url = Settings.Values.GetValue("LITE_LINK_DEEP_URL", "https://chat.faberlic.com/external/%code%")
                        .Replace("%code%", gen.Generate(mtu.ToTransport.User) ?? "ошибка");
                    if (html)
                        url = String.Concat("<a href='", url, "'>", url, "</a>");
                    return url;
                };
            }
            else
            {
                ReplaceDict["ссылка"] = mtu => "ссылка";
            }
            return new TagReplaceTextCompiler(ReplaceDict);
        }
    }
}
