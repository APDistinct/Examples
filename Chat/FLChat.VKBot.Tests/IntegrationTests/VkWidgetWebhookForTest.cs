using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FLChat.VKBot.VkWidget;
using Newtonsoft.Json;

namespace FLChat.VKBot.Tests.IntegrationTests
{
    public class VkWidgetWebhookForTest : VkWidgetWebhook
    {

        protected override string GetRequestString(HttpContext context)
        {
            var requestObject = new VkWebhookCallbackData
            {
                Id = 1,
                DeepLink = "12321321"
            };

            var requestMessage = JsonConvert.SerializeObject(requestObject);
            return requestMessage;
        }
    }
}
