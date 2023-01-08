using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Reports
{
    class CommonInfoReader
    {
        public static CommonInfo GetMessageCommonInfo(ChatEntities entities, Guid messageId)
        {
            var mess = entities.Message.Where(m => m.Id == messageId)
                .Include(m => m.FromTransport.User).FirstOrDefault();
            if (mess == null)
                return null;
            return new CommonInfo()
            {
                UserId = mess.FromUserId,
                FullName = mess.FromTransport.User.FullName,
                Phone = mess.FromTransport.User.Phone,
                MsgId = mess.Id,
                MessageTypeId = mess.MessageTypeId,
                Text = mess.Text,
                PostTm = mess.PostTm,
            };
        }
    }
}
