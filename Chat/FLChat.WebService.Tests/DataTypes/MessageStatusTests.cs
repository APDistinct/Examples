using System;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class MessageStatusTests
    {
        [TestMethod]
        public void MessageStatus_FromMessageToUser() {
            MessageToUser to = new MessageToUser() {
                IsDelivered = false,
                IsFailed = false,
                IsRead = false,
                IsSent = false,
                Message = new Message() {
                    IsDeleted = false,
                    DalayedCancelled = null,
                }
            };
            Assert.AreEqual(MessageStatus.Quequed, to.GetMessageStatus());
            to.Message.DalayedCancelled = DateTime.UtcNow;
            Assert.AreEqual(MessageStatus.Cancelled, to.GetMessageStatus());
            to.IsSent = true;            
            Assert.AreEqual(MessageStatus.Sent, to.GetMessageStatus());
            to.IsDelivered = true;
            Assert.AreEqual(MessageStatus.Delivered, to.GetMessageStatus());
            to.IsRead = true;
            Assert.AreEqual(MessageStatus.Read, to.GetMessageStatus());
            to.IsFailed = true;
            Assert.AreEqual(MessageStatus.Failed, to.GetMessageStatus());
            to.Message.IsDeleted = true;
            Assert.AreEqual(MessageStatus.Deleted, to.GetMessageStatus());
        }
    }
}
