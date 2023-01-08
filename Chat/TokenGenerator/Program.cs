using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;
using FLChat.WebService.Handlers.Auth;

namespace TokenGenerator
{
    class Program
    {
        static void Main(string[] args) {
            if (args.Length == 0)
                Console.WriteLine("First argument must be User's id or phone");

            int expireBy = 60 * 60 * 24 * 90;

            using (ChatEntities entities = new ChatEntities()) {
                string token;
                AuthTokenFactory f = new AuthTokenFactory();
                DateTime dt = DateTime.Now;
                bool isGuid = false;
                if (Guid.TryParse(args[0], out Guid guid)) {
                    isGuid = true;
                } else {
                    string phone = args[0];
                    User user = entities.User.Where(u => u.Phone == phone).SingleOrDefault();
                    if (user == null) {
                        Console.WriteLine("User has't found");
                        return;
                    }
                    guid = user.Id;
                }

                if (guid == Guid.Empty)
                    expireBy = 60 * 60 * 24 * 365 * 20;
                token = f.Gen(guid, dt, expireBy);
                entities.AuthToken.Add(new AuthToken() {
                    Token = token,
                    ExpireBy = expireBy,
                    IssueDate = dt,
                    UserId = guid
                });
                entities.SaveChanges();

                using (StreamWriter file = System.IO.File.AppendText("tokens.txt")) {
                    file.WriteLine(guid.ToString());
                    if (!isGuid)
                        file.WriteLine(args[0]);
                    file.WriteLine(token);
                    file.WriteLine();
                }
            }
        }
    }
}
