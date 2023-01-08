using FLChat.DAL.Model;
using FLChat.WebService.MediaType;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace AvatarCutter
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger("root");

        static void Main(string[] args)
        {
            log.Info("Start avatar's cutting");
            try
            {
                AvatarChanger changer = new AvatarChanger();
                int num = 0;
                using (ChatEntities entities = new ChatEntities())
                {                    
                    var users = entities.User.Where(u => u.AvatarUploadDate != null)
                        .Include(u => u.UserAvatar)
                        .ToList();

                    log.Info($"Number of users with avatar: {users.Count}");
                    foreach (var user in users)
                    {
                        try
                        {
                            int ret = changer.Change(entities, user.UserAvatar);
                            if (ret < 0)
                            {
                                string s = "User " + (user.FullName ?? "") + $"  id: {user.Id}  bad avatar image data";
                                log.Error(s);
                            }
                            else
                            {
                                num += ret;
                            }
                            entities.SaveChanges();
                        }
                        catch(Exception ex)
                        {
                            string s = "User " + (user.FullName ?? "") + $"  id: {user.Id}  ";
                            log.Error(s );  //  log.Error(s + ex.ToString());
                            log.Error(ex.ToString());
                        }
                    }
                }
                log.Info($"Number of cutting avatars: {num}");
            }
            catch (Exception e) 
            {
                log.Error(e.ToString());
            }
}
    }
}
