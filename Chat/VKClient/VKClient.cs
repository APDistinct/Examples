using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading;


namespace VKAccess
{
    public class VKClient
    {
        private string _accessToken;
        private Uri _baseUri;
        private string version = "5.92";
        private string strCommand;
        private string strParam;

        public int errorCode { get; set; }
        public string errorMsg { get; set; }
        public const string DefaultBaseUrl = "https://api.vk.com/method/";

        /// <summary>
        /// Profile response data
        /// </summary>
        public class Profile
        {
            [JsonProperty("user_id")]
            public string UserId { get; set; }
        }        

        public VKClient(string accessToken, string baseUrl = DefaultBaseUrl)
        {
            _baseUri = new Uri(baseUrl);
            _accessToken = accessToken;                 
        }

        /// <summary>
        /// Perform GetProfile HTTP request 
        /// </summary>
        /// <param name="userIdOrPhone"></param>
        /// <returns></returns>
        public async Task<Info> GetInfo(/*string userIdOrPhone, */CancellationToken ct)
        {
            strCommand = "account.getInfo?";
            strParam = "";
            Info retInfo = null;
            //try
            {
                var jsonstr = await SendGetRequest(ct);
                // Проверить на ошибку
                retInfo = ExtractResponse<Info>(jsonstr);
            }
            //catch(Exception e)
            {
                // Разобрать ошибку

            }
            //string ss = ret.Country;
            return retInfo;  // ExtractData<Info>(json);

        }
        public async Task<string> SendMessage(int userId, string msg, CancellationToken ct)
        {
            Random rnd = new Random();
            int random_id = rnd.Next();
            strCommand = "messages.send?";
            strParam = $"user_id={userId}&message={msg}&random_id={random_id}";

            string retInfo;
            var jsonstr = await SendGetRequest(ct);
            retInfo = ExtractResponse<string>(jsonstr);

            return retInfo;

        }

        public async Task<string> SendGetRequest(/*string userIdOrPhone, */CancellationToken ct)
        {
            using (HttpClient client = new HttpClient())
            {
                string ss = $"{strCommand}access_token={_accessToken}&v={version}";
                if (strParam.Any())
                {
                    ss +=  $"&{strParam}";
                }
                Uri uri = new Uri(_baseUri, ss);
                
                HttpResponseMessage response = await client.GetAsync(uri, ct);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    errorCode = response.StatusCode.GetHashCode();
                    errorMsg = response.StatusCode.ToString();
                    throw new HttpRequestException($"GetProfile received status code {response.StatusCode.ToString()}");
                }
                string data = await response.Content.ReadAsStringAsync();                
                
                return data;
            }
        }

        private T ExtractResponse<T>(string data)
        {
            JObject json = JObject.Parse(data);
            if (data.StartsWith("{\"error\""))
            {
                //var error = JsonConvert.DeserializeObject<ExecuteError>(data);
                //msg.Success = false;
                //msg.ErrorMsg = $"Get groups error. Code: {error.ErrorCode}, message: {error.ErrorMsg}";
                var err = ((JObject)json[JsonFields.Error]).ToObject<ExecuteError>();
                errorCode = err.ErrorCode;
                errorMsg = err.ErrorMsg;
                throw new Exception($"{errorCode}  {err.ErrorMsg}");
                //return default(T); 
            }
            //try
            //{
                
                
            //    var errt = (JObject)json[JsonFields.Error];
            //    var err = ((JObject)json[JsonFields.Error]).ToObject<ExecuteError>();
            //    int error = err.ErrorCode;
            //    if (error != 0)
            //        throw new Exception($"{error}  {err.ErrorMsg}]");
            //    return default(T);
            //}
            //catch(Exception e)
            //{
            //    Console.WriteLine($"Ошибка  {e.Message}");
            //}
            
            return ((JObject)json[JsonFields.Response]).ToObject<T>();
        }


        /// <summary>
        /// Perform GetProfile HTTP request 
        /// </summary>
        /// <param name="userIdOrPhone"></param>
        /// <returns></returns>
        public async Task<Info> SendMessage(/*string userIdOrPhone, */CancellationToken ct)
        {
            using (HttpClient client = new HttpClient())
            {
                Uri uri = new Uri(_baseUri, $"account.getInfo?access_token={_accessToken}&v={version}");
                HttpResponseMessage response = await client.GetAsync(uri, ct);
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new HttpRequestException($"GetProfile received status code {response.StatusCode.ToString()}");

                string data = await response.Content.ReadAsStringAsync();
                //JObject json = JObject.Parse(data);
                var ret = ExtractResponse<Info>(data);
                string ss = ret.Country;
                return ret;  // ExtractData<Info>(json);
            }
        }
        /// <summary>
        /// Perform GetProfile HTTP request 
        /// </summary>
        /// <param name="userIdOrPhone"></param>
        /// <returns></returns>
        public async Task<Profile> GetProfile(string userIdOrPhone, CancellationToken ct)
        {
            using (HttpClient client = new HttpClient())
            {
                Uri uri = new Uri(_baseUri, $"getprofile?access_token={_accessToken}&data={{\"user_id\":\"{userIdOrPhone}\"}}");
                HttpResponseMessage response = await client.GetAsync(uri, ct);
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new HttpRequestException($"GetProfile received status code {response.StatusCode.ToString()}");

                string data = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(data);
                return ExtractData<Profile>(json);
            }
        }

        /// <summary>
        /// Send text message to single user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        //public async Task<string> SendMessage(string userId, string msg, CancellationToken ct)
        //{
        //    SendMessageData requestData = new SendMessageData(userId, msg);
        //    HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8);
        //    //httpContent.Headers.Add("Content-Type", "application/json");
        //    using (HttpClient client = new HttpClient()) {
        //        Uri uri = new Uri(_baseUri, $"message?access_token={_accessToken}");
        //        HttpResponseMessage response = await client.PostAsync(uri, httpContent, ct);
        //        if (response.StatusCode != HttpStatusCode.OK)
        //            throw new HttpRequestException($"SendMessage received status code {response.StatusCode.ToString()}");

        //        string data = await response.Content.ReadAsStringAsync();
        //        JObject json = JObject.Parse(data);
        //        JObject obj = ExtractData<JObject>(json);
        //        return (string)obj[JsonFields.MessageId];
        //    }
        //}

        /// <summary>
        /// data class for send message request
        /// </summary>
        private class SendMessageData
        {
            public  class RecipientData
            {
                public string user_id { get; set; }
            }
            public class MessageData {
                public string text { get; set; }
            }

            public SendMessageData(string userId, string msg) {
                recipient.user_id = userId;
                message.text = msg;
            }

            public RecipientData recipient { get; } = new RecipientData();
            public MessageData message { get; } = new MessageData();
        }

        /// <summary>
        /// Verify response error code and throw exception if error != 0
        /// Extract field 'data' and returns it
        /// </summary>
        /// <param name="json">zalo service response</param>
        /// <exception cref="ZaloException">throw exception if error != 0</exception>
        private T ExtractData<T>(JObject json) {
            int error = (int)json[JsonFields.Error];
            if (error != 0)
                throw new Exception($"error  (string)json[JsonFields.Message]");

            return ((JObject)json[JsonFields.Data]).ToObject<T>();
        }
        
        /// <summary>
        /// Json fields
        /// </summary>
        private static class JsonFields
        {
            public const string Response = "response";
            public const string Error = "error";
            public const string Data = "data";
            public const string Message = "message";
            //public const string UserId = "user_id";

            public const string MessageId = "message_id";
        }
    }
}
