using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.DAL.Model.Tests
{
    [TestClass]
    public class GroupTests
    {
        ChatEntities entities;

        [TestInitialize]
        public void Init()
        {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Clean()
        {
            entities.Dispose();
        }

        [TestMethod]
        public void Group_Create()
        {
            Group g = new Group()
            {
                CreatedByUserId = entities.GetUser(u => true, null).Id
            };
            entities.Group.Add(g);
            entities.SaveChanges();

            Assert.AreNotEqual(Guid.Empty, g.Id);
            Assert.IsFalse(g.IsDeleted);
            Assert.IsTrue(Math.Abs((g.CreatedDate - DateTime.UtcNow).TotalMinutes) < 1);

            entities.Entry(g).State = System.Data.Entity.EntityState.Deleted;
            entities.SaveChanges();
        }

        [TestMethod]
        public void GroupAddAdminsTest()
        {
            User[] users = entities.GetUsers(3, u => u.Enabled, u => { });
            Group group = new Group()
            {
                CreatedByUserId = users[0].Id
            };
            entities.Group.Add(group);
            Guid[] guids = new Guid[2];
            guids[0] = users[1].Id;
            guids[1] = users[2].Id;

            //  Метод
            group.AddAdmins(guids);
            entities.SaveChanges();

            //  Проверка
            MakeProofAdd(group, users.Skip(1).ToArray(), true);

            //  Зачистка
            
            group.Members.Clear();
            entities.Entry(group).State = System.Data.Entity.EntityState.Deleted;
            entities.SaveChanges();
        }

        [TestMethod]
        public void GroupAddMembersTest()
        {
            User[] users = entities.GetUsers(3, u => u.Enabled, u => { });
            Group group = new Group()
            {
                CreatedByUserId = users[0].Id
            };
            entities.Group.Add(group);
            Guid[] guids = new Guid[2];
            guids[0] = users[1].Id;
            guids[1] = users[2].Id;

            //  Метод
            group.AddMembers(guids);
            entities.SaveChanges();

            //  Проверка
            MakeProofAdd(group, users.Skip(1).ToArray(), false);

            //  Зачистка
            
            group.Members.Clear();
            entities.Entry(group).State = System.Data.Entity.EntityState.Deleted;
            entities.SaveChanges();
        }

        private void MakeProofAdd(Group group, User[] users, bool isAdm)
        {
            int num = users.Count();
            
            for (int i = 0; i < num; ++i)
            {
                var grm = group.Members.Where(x => x.UserId == users[i].Id).FirstOrDefault();                
                Assert.IsNotNull(grm);
                Assert.AreEqual(grm.IsAdmin, isAdm);                
            }

        }

        [TestMethod]
        public void GroupDelAdminsTest()
        {
            bool isAdmin = true;
            User[] users = entities.GetUsers(3, u => u.Enabled, u => { });
            Group group = new Group()
            {
                CreatedByUserId = users[0].Id
            };
            entities.Group.Add(group);
            Guid[] guids = new Guid[2];
            guids[0] = users[1].Id;
            guids[1] = users[2].Id;

            for (int i = 1; i < users.Count(); ++i)
            {
                var g_mem = new GroupMember() { IsAdmin = isAdmin, UserId = users[i].Id };
                group. Members.Add(g_mem);                
            }

            // Метод
            group.DelAdmins(guids);
            entities.SaveChanges();

            //  Проверка
            MakeProofDelAdm(group, users.Skip(1).ToArray(), false);

            //  Зачистка
            
            group.Members.Clear();
            entities.Entry(group).State = System.Data.Entity.EntityState.Deleted;
            entities.SaveChanges();
        }

        private void MakeProofDelAdm(Group group, User[] users, bool isAdm)
        {
            int num = users.Count();
            
            for (int i = 0; i < num; ++i)
            {
                var grm = group.Members.Where(x => x.UserId == users[i].Id).FirstOrDefault();
                if (grm != null)
                {
                    Assert.AreEqual(grm.IsAdmin, isAdm);
                    //Assert.AreEqual(grm.UserId, users[i].Id);
                }
            }

        }

        [TestMethod]
        public void GroupDelMembersTest()
        {
            //  Подготовка
            bool isAdmin = false;
            User[] users = entities.GetUsers(3, u => u.Enabled, u => { });
            Group group = new Group()
            {
                CreatedByUserId = users[0].Id
            };
            entities.Group.Add(group);
            Guid[] guids = new Guid[2];
            guids[0] = users[1].Id;
            guids[1] = users[2].Id;

            for (int i = 1; i < users.Count(); ++i)
            {
                var g_mem = new GroupMember() { IsAdmin = isAdmin, UserId = users[i].Id };
                group.Members.Add(g_mem);
            }

            //  Метод
            group.DelMembers(guids);
            entities.SaveChanges();

            //  Проверка
            MakeProofDelMem(group, users.Skip(1).ToArray());

            //  Зачистка
            
            group.Members.Clear();
            entities.Entry(group).State = System.Data.Entity.EntityState.Deleted;
            entities.SaveChanges();
        }

        private void MakeProofDelMem(Group group, User[] users)
        {
            int num = users.Count();

            for (int i = 0; i < num; ++i)
            {
                var grm = group.Members.Where(x => x.UserId == users[i].Id).FirstOrDefault();
                Assert.IsNull(grm);                
            }

        }

    }
}
