using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.VKBotClient.AttachmentManager
{
    public class VkApiConfiguration
    {
        public string Token { get; set; } = "49fbc63ef10dc50c92455fcf35537aa2f2722aeafb9d06d216aeb3bcd9c2e19b5546301bdfdd8529abcb3";  //
        public string DefaultBaseUrl { get; set; } = "https://api.vk.com/method/";
        public string Version { get; set; } = "5.92";
        public string UploadServer { get; set; }
        public string Save { get; set; }
        public string PeerId { get; set; } = "179649792";
    }
}
