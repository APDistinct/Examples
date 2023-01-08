using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VKAccess;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace Chat
{
    public partial class Form1 : Form
    {
        private VKClient vkClient;
        //private CancellationToken ct = new CancellationToken();
        //private long applicationId = 6888569;
        private int userId = /*244960181; //*/534672230;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonConnect_ClickAsync(object sender, EventArgs e)
        {
             StartSeesionAsync();
        }
        private void StartSeesionAsync()
        {
            string strId = textBoxId.Text;
            string strTo = textBoxToken.Text;


            //VkApi api = new VkApi();            
            try
            {
                vkClient = new VKClient(strTo);
                
                //api.Authorize(new ApiAuthParams()
                //{
                //    Login = "+79145671450",
                //    Password = "awer1234AWER",
                //    ApplicationId = 6888569,
                //    Settings = Settings.All
                //});
            }
            catch (Exception e)
            {
                string s = e.Message;
                return;
            }
            //var dialogs = api.Messages.GetConversations(new GetConversationsParams());
            //var messages = api.Messages.GetHistory(new MessagesGetHistoryParams()
            //{ PeerId = dialogs.Items[0].Conversation.Peer.Id });
            //Console.OutputEncoding = Encoding.UTF8;
            //foreach (var msg in messages.Messages)
            //{
            //    Console.WriteLine(msg.Text);
            //}

            //api.Messages.Send(new MessagesSendParams()
            //{
            //    ChatId = api.UserId.Value,
            //    Message = "message",
            //    //PeerId = messages.Messages.First().Id,
            //    //Message = "Test",
            //    //RandomId = new Random().Next()
            //});


        }
        private async void SendMessAsync()
        {
            try
            {
                string msg = textBoxMess.Text;
                string userid = userId.ToString();
                string ret = await vkClient.SendMessage(userId, msg, CancellationToken.None);
                MessageBox.Show($"Сработало - код {ret}");
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void buttonSend_Click(object sender, EventArgs e)
        {
            SendMessAsync();
        }

        private async void www()
        {
            try
            {
                var info = await vkClient.GetInfo(CancellationToken.None);
                string s = info.Country;
                MessageBox.Show($"Сработало - страна {s}");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void buttonInfo_ClickAsync(object sender, EventArgs e)
        {
            www();            
        }
    }
}
