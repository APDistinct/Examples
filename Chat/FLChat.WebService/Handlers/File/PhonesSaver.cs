using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.WebService.Handlers.File
{
    public interface IPhonesSaver
    {
        void Save(ChatEntities entities, Guid userId, IEnumerable<Guid> userIds);
    }

    public class PhonesSaver : IPhonesSaver
    {
        public void Save(ChatEntities entities, Guid userId, IEnumerable<Guid> userIds)
        {
            var user = entities.User.FirstOrDefault(a => a.Id == userId);
            if (user == null)
                throw new Exception($"User with Id {userId.ToString()} isn't found");

            user.MatchedPhonesAddr.Clear();
            entities.SaveChanges();

            var addUsers = entities.User.Where(a => userIds.Contains(a.Id));

            foreach (var addUser in addUsers)
            {
                user.MatchedPhonesAddr.Add(addUser);
            }

            entities.SaveChanges();
        }
    }
}
