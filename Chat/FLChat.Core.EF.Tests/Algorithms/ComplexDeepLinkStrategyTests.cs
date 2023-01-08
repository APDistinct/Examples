using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL.Model;

namespace FLChat.Core.Algorithms.Tests
{
    [TestClass]
    public class ComplexDeepLinkStrategyTests
    {
        [TestMethod]
        public void ComplexDeepLinkStrategyTest() {
            ActionDeepLinkStrategy[] list = new ActionDeepLinkStrategy[] {
                new ActionDeepLinkStrategy() { Result = false },
                new ActionDeepLinkStrategy() { Result = false }
            };
            ComplexDeepLinkStrategy strategy = new ComplexDeepLinkStrategy(list);
            User user;
            Message answerTo;
            object context;
            IDeepLinkStrategy sender;
            bool result;
            result = strategy.AcceptDeepLink(null, null, out user, out answerTo, out context, out sender);
            Assert.IsFalse(result);
            Assert.IsNull(user);
            Assert.IsNull(context);
            Assert.IsNull(sender);

            list[0].Result = true;
            list[0].User = new User();
            result = strategy.AcceptDeepLink(null, null, out user, out answerTo, out context, out sender);
            Assert.IsTrue(result);
            Assert.AreSame(list[0].User, user);
            Assert.AreSame(list[0].Context, context);
            Assert.AreSame(list[0], sender);

            list[1].Result = true;
            list[1].User = new User();
            result = strategy.AcceptDeepLink(null, null, out user, out answerTo, out context, out sender);
            Assert.IsTrue(result);
            Assert.AreSame(list[0].User, user);
            Assert.AreSame(list[0].Context, context);
            Assert.AreSame(list[0], sender);

            list[0].Result = false;
            list[0].User = null;
            result = strategy.AcceptDeepLink(null, null, out user, out answerTo, out context, out sender);
            Assert.IsTrue(result);
            Assert.AreSame(list[1].User, user);
            Assert.AreSame(list[1].Context, context);
            Assert.AreSame(list[1], sender);
        }
    }
}
