using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace DevinoTest.DevinoEmailService
{
    public class EmailService
    {
        private const string Host = "integrationapi.net/email/v2";
        private const string Scheme = "https";

        private HttpClient Client = new HttpClient();

        public EmailService(string login, string password)
        {
            var byteArray = Encoding.ASCII.GetBytes($"{login}:{password}");
            var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            Client.DefaultRequestHeaders.Authorization = header;
        }

        /// <summary>
        /// Получение адресов отправителя
        /// GET /UserSettings/SourceAddresses
        /// Метод возвращает адреса отправителя авторизованного пользователя - подтверждённые и запрошенные.
        /// https://devino-documentation.readthedocs.io/emailhttp.html#id7
        /// </summary>
        /// <returns></returns>
        public async Task<string> SourceAddresses()
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["format"] = "json";
            var requestUri = $"{Scheme}://{Host}/UserSettings/SourceAddresses?{queryString}";
            return await GetAsync(requestUri);
        }

        /// <summary>
        /// Отправка транзакционного сообщения
        /// POST v2/messages
        /// Метод отправляет транзакционное сообщение нескольким получателям с возможностью использования макросов. Если сообщение успешно добавлено в очередь, возвращается код «ok» и http код 201. В качестве Result возвращается идентификатор сообщения (string).
        /// http://docs.devinotele.com/emailhttp.html#id25
        /// </summary>
        /// <returns></returns>
        public async Task<string> Messages()
        {
            var content = new
            {
                Sender = new
                {
                    Address = "faberlicchat@faberlic.com",
                    Name = "Test"
                },
                Recipients = new[]
                {
                    new
                    {
                        MergeFields = new
                        {
                            ExtField = "5 дней",
                            Name = "Vova"
                        },
                        RecipientId = "",
                        Address = "dvikdvik@gmail.com",
                        Name = "Vova"
                    }
                },
                Subject = "Ув. [Name]!",
                Body = new
                {
                    Html = "Ув. [Name]! Осталось [ExtField]<br><a href=\"[Unsubscribe]\">Отписаться</a>",
                    PlainText = "Ув. {ExtField}! Ждем вас завтра! [Unsubscribe]"
                },
                //UserCampaignId = "1234"
            };

            var jsonString = JsonConvert.SerializeObject(content);

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["format"] = "json";
            //using (var client = new HttpClient())
            {
                var response = await Client.PostAsync(
                    $"{Scheme}://{Host}/messages?{queryString}",
                    new StringContent(jsonString, Encoding.UTF8, "application/json"));

                return await response.Content.ReadAsStringAsync();
            }
        }

        private async Task<string> GetAsync(string requestUri)
        {
            //using (var client = new HttpClient())
            {
                var response = await Client.GetAsync(requestUri);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();

                throw new Exception(response.StatusCode.ToString());
            }
        }

    }
}
