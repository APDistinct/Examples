using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FLChat.Core;
using FLChat.DAL.Model;
using FLChat.VKBotClient.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.VKBot.Tests
{
    [TestClass]
    public class GetUserInfoTests
    {
        [TestMethod]
        [Ignore("Отключен поскольку содержит персональные пользователи юзера которых может не быть")]
        public async Task GetUserInfoTest()
        {
            CancellationToken ct = new CancellationToken();
            var client = new VKBotClient.VKBotClient("0be1ca4d1f355216067f88a68a3f57a5a8267d3b2b336e5fea1cf6235880203f118c9e346dbf173f07271");

            var response = await client.GetUserInfoAsync("589103673", ct);
            Assert.IsNotNull(response.User.First().AvatarUrl);
        }
    }
}