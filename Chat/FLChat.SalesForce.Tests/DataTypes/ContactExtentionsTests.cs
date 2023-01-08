using System;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using System.Data;
using System.Collections.Generic;

namespace FLChat.SalesForce.DataTypes.Tests
{
    [TestClass]
    public class ContactExtentionsTests
    {
        [TestMethod]
        public void ToImportUsersSalesForceTable() {
            string json = File.ReadAllText(".\\json\\contact.json");
            Contact contact = new Contact() {
                Birthdate = DateTime.Now - TimeSpan.FromDays(1000),
                BonusPoints = 123.5,
                City = "Xuzhou",
                Region = "Jiansu",
                Country = "China",
                CreatedDate = DateTime.Now - TimeSpan.FromDays(900),
                Email = "somemail@ya.ru",
                EmailPermission = true,
                FirstName = "Slon",
                Id = "1234rty",
                IsDeleted = false,
                LastName = "Crocodile",
                LastOrderDate = DateTime.Now - TimeSpan.FromDays(500),
                Phone = "1234456-00",
                PhotoUrl = "url",
                ReportsToId = "445566uu",
                SMSPermission = true,
                Title = "Duke"
            };
            Contact empty = new Contact() {
                Id = "hgx",
                CreatedDate = DateTime.Now
            };
            DataTable dt = new Contact[] { contact, empty }.ToImportUsersSalesForceTable();

            Assert.AreEqual(2, dt.Rows.Count);
            contact.Phone = "123445600";
            AreEqual(contact, dt.Rows[0], 0);
            AreEqual(empty, dt.Rows[1], 1);
        }

        [TestMethod]
        public void ParseSegmentsTest() {
            Contact c = new Contact() { Segments = "abc;tyc" };
            CollectionAssert.AreEqual(new string[] { "abc", "tyc" }, c.ParseSegments());

            c.Segments = null;
            Assert.IsNull(c.ParseSegments());
        }

        [TestMethod]
        public void CreateImportSegmentsTablesTest() {
            Contact[] list = new Contact[] {
                new Contact() { Id = "1a", Segments = null },
                new Contact() { Id = "2a", Segments = "abc;xyz" },
                new Contact() { Id = "3a", Segments = "abc" }
            };
            Dictionary<string, Guid> dict = new Dictionary<string, Guid>() {
                { "abc", Guid.NewGuid() },
                { "xyz", Guid.NewGuid() }
            };

            list.CreateImportSegmentsTables(s => dict[s], out DataTable users, out DataTable segments);

            Assert.AreEqual(3, users.Rows.Count);
            Assert.AreEqual(1, users.Columns.Count);
            CollectionAssert.AreEqual(
                new string[] { "1a", "2a", "3a" },
                new string[] { (string)users.Rows[0][0], (string)users.Rows[1][0], (string)users.Rows[2][0] });

            Assert.AreEqual(3, segments.Rows.Count);
            Assert.AreEqual(2, segments.Columns.Count);
            Assert.AreEqual("2a", (string)segments.Rows[0][0]);
            Assert.AreEqual(dict["abc"], (Guid)segments.Rows[0][1]);
            Assert.AreEqual("2a", (string)segments.Rows[1][0]);
            Assert.AreEqual(dict["xyz"], (Guid)segments.Rows[1][1]);
            Assert.AreEqual("3a", (string)segments.Rows[2][0]);
            Assert.AreEqual(dict["abc"], (Guid)segments.Rows[2][1]);
        }

        private void AreEqual(Contact contact, DataRow row, int rn) {
            AreEqual(contact.LastName, row["SURNAME"]);
            AreEqual(contact.FirstName, row["NAME"]);
            AreEqual<string>(null, row["PATRONYMIC"]);
            AreEqual(contact.Birthdate, row["BIRTHDAY"]);
            AreEqual(contact.Phone, row["MOBILE"]);
            AreEqual(contact.Email, row["EMAIL"]);
            AreEqual(contact.Title, row["TITLE"]);
            AreEqual(contact.Country, row["COUNTRY"]);
            AreEqual(contact.Region, row["REGION"]);
            AreEqual(contact.City, row["CITY"]);
            AreEqual((DateTime?)contact.CreatedDate, row["REGISTRATIONDATE"]);
            AreEqual(contact.EmailPermission, row["EMAILPERMISSION"]);
            AreEqual(contact.SMSPermission, row["SMSPERMISSION"]);
            AreEqual(contact.LastOrderDate, row["LASTORDERDATE"]);
            AreEqual((decimal?)contact.BonusPoints, row["FLCLUBPOINTS"]);
            AreEqual((int?)rn, row["ROWNUMBER"]);
            AreEqual(contact.Id, row["ForeignID"]);
            AreEqual(contact.ReportsToId, row["ForeignOwnerID"]);
            AreEqual((bool?)!contact.IsDeleted, row["Enabled"]);
        }
        
        private void AreEqual<T>(T expected, object obj) where T : class{
            if (expected != null)
                Assert.AreEqual(expected, (T)obj);
            else
                Assert.AreEqual(DBNull.Value, obj);
        }

        private void AreEqual<T>(T? expected, object obj) where T : struct {
            if (expected.HasValue)
                Assert.AreEqual(expected.Value, (T?)obj);
            else
                Assert.AreEqual(DBNull.Value, obj);
        }
    }
}
