using System;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.SalesForce.DataTypes.Tests
{
    [TestClass]
    public class ContactTests
    {
        [TestMethod]
        public void FieldNames() {
            Contact contact = new Contact() {
                Id = "123"
            };
            string json = JsonConvert.SerializeObject(contact);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "Birthdate", "BonusPoints__c", "CreatedDate", "Email", "EmailPermission__c",
                    "FirstName", "Id", "IsDeleted", "LastName", "LastOrderDate__c", "MailingCity",
                    "MailingCountry", "MailingState", "MobilePhone", "PhotoUrl", "ReportsToId",
                    "SMSPermission__c", "Title", "Segment__c"
                },
                jo.Properties().Select(p => p.Name).ToArray());
        }

        [TestMethod]
        public void Deserialize() {
            string json = File.ReadAllText(".\\json\\contact.json");
            Contact contact = JsonConvert.DeserializeObject<Contact>(json);
            Assert.AreEqual(new DateTime(1980, 04, 12), contact.Birthdate.Value);
            Assert.AreEqual(104.0, contact.BonusPoints);
            Assert.IsTrue(Math.Abs((new DateTime(2019, 12, 09, 08, 07, 13, DateTimeKind.Utc) 
                - contact.CreatedDate.ToUniversalTime()).TotalSeconds) < 1);
            Assert.AreEqual("as.analytics@yandex.ru", contact.Email);
            Assert.IsTrue(contact.EmailPermission.Value);
            Assert.AreEqual("Anna", contact.FirstName);
            Assert.AreEqual("0036g0000047EQvAAM", contact.Id);
            Assert.IsFalse(contact.IsDeleted);
            Assert.AreEqual("Cole", contact.LastName);
            Assert.IsTrue(Math.Abs((new DateTime(2019, 12, 09, 09, 07, 13, DateTimeKind.Utc) 
                - contact.LastOrderDate.Value.ToUniversalTime()).TotalSeconds) < 1);
            Assert.AreEqual("Moscow", contact.City);
            Assert.AreEqual("Russia", contact.Country);
            Assert.AreEqual("Moscow", contact.Region);
            Assert.AreEqual("79162842692", contact.Phone);
            Assert.AreEqual("/services/images/photo/0036g0000047EQvAAM", contact.PhotoUrl);
            Assert.AreEqual("1234", contact.ReportsToId);
            Assert.IsTrue(contact.SMSPermission.Value);
            Assert.AreEqual("Client", contact.Title);
            Assert.AreEqual("Newcomers;Gold;Silver;Bronze;Campaign participans", contact.Segments);
        }

        [TestMethod]
        public void DeserializeEmpty() {
            string json = File.ReadAllText(".\\json\\contact_empty.json");
            Contact contact = JsonConvert.DeserializeObject<Contact>(json);
            Assert.IsNull(contact.Birthdate);
            Assert.IsNull(contact.BonusPoints);
            Assert.IsTrue(Math.Abs((new DateTime(2019, 12, 06, 11, 53, 13, DateTimeKind.Utc)
                - contact.CreatedDate.ToUniversalTime()).TotalSeconds) < 1);
            Assert.IsNull(contact.Email);
            Assert.IsNull(contact.EmailPermission);
            Assert.AreEqual("Geoff", contact.FirstName);
            Assert.AreEqual("0036g000004A6ydAAC", contact.Id);
            Assert.IsFalse(contact.IsDeleted);
            Assert.AreEqual("Minor", contact.LastName);
            Assert.IsNull(contact.LastOrderDate);
            Assert.IsNull(contact.City);
            Assert.IsNull(contact.Country);
            Assert.IsNull(contact.Region);
            Assert.IsNull(contact.Phone);
            Assert.IsNull(contact.PhotoUrl);
            Assert.IsNull(contact.ReportsToId);
            Assert.IsNull(contact.SMSPermission);
            Assert.IsNull(contact.Title);
            Assert.IsNull(contact.Segments);
        }
    }
}
