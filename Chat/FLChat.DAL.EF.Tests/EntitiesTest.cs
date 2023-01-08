using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;

using FLChat.DAL.Model;
//using FLChat.WebService.DataTypes;

namespace FLChat.DAL.Model.Tests
{
    [TestClass]
    public class EntitiesTest
    {
        ChatEntities entities;        
        User from;

        [TestInitialize]
        public void Init()
        {
            entities = new ChatEntities();            

            from = entities.GetUser(
                u => u.Enabled,
                u => u.Enabled = true);
        }

        [TestCleanup]
        public void Clean()
        {
            entities.Dispose();
        }
    }
}
