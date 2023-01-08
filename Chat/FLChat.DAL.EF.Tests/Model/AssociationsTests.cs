using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using FLChat.DAL.Model;
using FLChat.DAL.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.DAL.Model.Tests
{
    [TestClass]
    public class AssociationsTests
    {
        ChatEntities entities;
        //Guid from;
        User[] to;
        User user;

        [TestInitialize]
        public void Init()
        {
            entities = new ChatEntities();
            user = entities.GetUserQ();
            to = entities.GetUsers(2, u => u.Id != user.Id, null);

            //from = Guid.NewGuid();
            
        }

        [TestMethod]
        public void MatchedPhonesTest_Addr()
        {
            int n = user.MatchedPhonesSender.Count;
            n = user.MatchedPhonesAddr.Count;
            //user.MatchedPhonesSender.Clear();
            user.MatchedPhonesAddr.Clear();
            entities.SaveChanges();
            entities.Entry(user).Reload();
            n = user.MatchedPhonesSender.Count;

            using (var opener = new ConnectionOpener(entities))
            using (var cmd = entities.Database.Connection.CreateCommand())
            {
                //cmd.Transaction = entities.Database.CurrentTransaction.UnderlyingTransaction;
                cmd.CommandText = $"insert into [Usr].[MatchedPhones] values ('{user.Id.ToString()}','{to[0].Id.ToString()}')";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"insert into [Usr].[MatchedPhones] values ('{user.Id.ToString()}','{to[1].Id.ToString()}')";
                cmd.ExecuteNonQuery();
            }
            entities.Entry(user).Reload();
            entities.Entry(user).Collection(u => u.MatchedPhonesAddr).Load();
            n = user.MatchedPhonesSender.Count;
            //User user = entities.User.Where(x => x.Id == from).SingleOrDefault();
            Assert.IsTrue(user.MatchedPhonesAddr.Count >= to.Count());
            Assert.IsTrue(user.MatchedPhonesAddr.Select(u => u.Id).Contains(to[0].Id));
            user.MatchedPhonesAddr.Clear();
            entities.SaveChanges();
        }
        [TestMethod]
        public void MatchedPhonesTest_Sender()
        {
            user.MatchedPhonesSender.Clear();
            entities.SaveChanges();
            entities.Entry(user).Reload();
         
            using (var opener = new ConnectionOpener(entities))
            using (var cmd = entities.Database.Connection.CreateCommand())
            {
                //cmd.Transaction = entities.Database.CurrentTransaction.UnderlyingTransaction;
                cmd.CommandText = $"insert into [Usr].[MatchedPhones] values ('{to[0].Id.ToString()}', '{user.Id.ToString()}')";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"insert into [Usr].[MatchedPhones] values ('{to[1].Id.ToString()}', '{user.Id.ToString()}')";
                cmd.ExecuteNonQuery();
            }
            entities.Entry(user).Reload();
            entities.Entry(user).Collection(u => u.MatchedPhonesSender).Load();
            Assert.IsTrue(user.MatchedPhonesSender.Count >= to.Count());
            Assert.IsTrue(user.MatchedPhonesSender.Select(u => u.Id).Contains(to[0].Id));
            user.MatchedPhonesSender.Clear();
            entities.SaveChanges();
        }

        [TestMethod]
        public void PersonalProhibitionTest_Main()
        {
            user.PersonalProhibitionMain.Clear();
            entities.SaveChanges();
            entities.Entry(user).Reload();         

            using (var opener = new ConnectionOpener(entities))
            using (var cmd = entities.Database.Connection.CreateCommand())
            {
                //cmd.Transaction = entities.Database.CurrentTransaction.UnderlyingTransaction;
                cmd.CommandText = $"insert into [Usr].[PersonalProhibition] values ('{to[0].Id.ToString()}', '{user.Id.ToString()}')";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"insert into [Usr].[PersonalProhibition] values ('{to[1].Id.ToString()}', '{user.Id.ToString()}')";                
                cmd.ExecuteNonQuery();
            }
            entities.Entry(user).Reload();
            entities.Entry(user).Collection(u => u.PersonalProhibitionMain).Load();
            
            Assert.IsTrue(user.PersonalProhibitionMain.Count >= to.Count());
            Assert.IsTrue(user.PersonalProhibitionMain.Select(u => u.Id).Contains(to[0].Id));
            user.PersonalProhibitionMain.Clear();
            entities.SaveChanges();
        }

        [TestMethod]
        public void PersonalProhibitionTest_Slave()
        {
            
            user.PersonalProhibitionSlave.Clear();
            entities.SaveChanges();            

            using (var opener = new ConnectionOpener(entities))
            using (var cmd = entities.Database.Connection.CreateCommand())
            {
                //cmd.Transaction = entities.Database.CurrentTransaction.UnderlyingTransaction;
                cmd.CommandText = $"insert into [Usr].[PersonalProhibition] values ('{user.Id.ToString()}','{to[0].Id.ToString()}')";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"insert into [Usr].[PersonalProhibition] values ('{user.Id.ToString()}','{to[1].Id.ToString()}')";
                cmd.ExecuteNonQuery();
            }
            entities.Entry(user).Reload();
            entities.Entry(user).Collection(u => u.PersonalProhibitionSlave).Load();
            
            Assert.IsTrue(user.PersonalProhibitionSlave.Count >= to.Count());
            Assert.IsTrue(user.PersonalProhibitionSlave.Select(u => u.Id).Contains(to[0].Id));
            user.PersonalProhibitionSlave.Clear();
            entities.SaveChanges();
        }
    }
}
