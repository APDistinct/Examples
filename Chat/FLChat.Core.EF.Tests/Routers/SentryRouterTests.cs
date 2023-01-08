using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL.Model;

namespace FLChat.Core.Routers.Tests
{
    [TestClass]
    public class SentryRouterTests
    {
        [TestMethod]
        public void SentryRouter_Test() {
            using (ChatEntities entities = new ChatEntities()) {
                UserSentry sentry = entities.UserSentry.FirstOrDefault();
                bool added = false;
                if (sentry == null) {
                    added = true;
                    User usr = entities.GetUser(u => u.Enabled == true && u.IsTemporary == false && u.IsBot == false, null);
                    sentry = entities.UserSentry.Add(new UserSentry() { UserId = usr.Id });
                    entities.SaveChanges();
                }

                SentryRouter router = new SentryRouter();
                Guid? id = router.RouteMessage(entities, null, null);

                //check value is not null
                Assert.IsNotNull(id);
                UserSentry dbsentry = entities.UserSentry.Where(u => u.UserId == id.Value).SingleOrDefault();
                //check returned user id contains in [UserSentry] table
                Assert.IsNotNull(dbsentry);

                if (added) {
                    entities.Entry(sentry).State = System.Data.Entity.EntityState.Deleted;
                    entities.SaveChanges();
                }
            }
        }
    }
}
