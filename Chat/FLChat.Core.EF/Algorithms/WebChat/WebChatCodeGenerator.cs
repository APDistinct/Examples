using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;

namespace FLChat.Core.Algorithms.WebChat
{
    public class WebChatCodeGenerator : IWebChatCodeGenerator
    {
        private static readonly Random _rnd = new Random();
        private static readonly object _lock = new object();
        private const string _chars = "abcdefghijklmnopqrstuvwxyz0123456789";

        public TimeSpan ExpireInDays { get; set; } = TimeSpan.FromDays(30);

        public int CodeLength { get; set; } = 20;

        public string Gen(MessageToUser mtu) {
            int tryCount = 0;
            while (true) {
                try {
                    string code = GenerateCode();

                    using (ChatEntities entities = new ChatEntities())
                    using (var trans = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted)) {
                        WebChatDeepLink item = entities.WebChatDeepLink.Add(new WebChatDeepLink() {
                            MsgId = mtu.MsgId,
                            ToUserId = mtu.ToUserId,
                            ToTransportTypeId = (int)TransportKind.WebChat,
                            ExpireDate = DateTime.UtcNow + ExpireInDays,
                            Link = code
                        });
                        entities.SaveChanges();
                        trans.Commit();

                        return item.Link;
                    }
                } catch (Exception e) {
                    if (e.ToString().Contains("UNQ__MsgWebChatDeepLink") == false || tryCount++ < 3)
                        throw;
                }
            }
        }

        private string GenerateCode() {
            lock (_lock) {                
                var stringChars = new char[CodeLength];

                for (int i = 0; i < stringChars.Length; i++) {
                    stringChars[i] = _chars[_rnd.Next(_chars.Length)];
                }

                return new string(stringChars);
            }
        }
    }
}
