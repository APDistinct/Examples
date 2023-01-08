using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FLChat.VKBotClient.Args;
using FLChat.VKBotClient.AttachmentManager;
using FLChat.VKBotClient.Callback;
using FLChat.VKBotClient.Exceptions;
using FLChat.VKBotClient.Requests.Abstractions;
using FLChat.VKBotClient.Requests.Available_Methods.GetUserInfo;
using FLChat.VKBotClient.Requests.Available_Methods.Sending_Messages;
using FLChat.VKBotClient.Requests.Available_Methods.Update;
using FLChat.VKBotClient.Response;
using FLChat.VKBotClient.Types;
using FLChat.VKBotClient.Types.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.VKBotClient
{
    public interface IVKBotClient
    {
        Task<SendMessagesResponse> SendTextMessageAsync(
            string userId,
            string text,
            CancellationToken cancellationToken,
            string kbString,
            IEnumerable<AttachmentFile> files);
        //Message SendTextMessage(string msgText);
    }


    public class VKBotClient : IVKBotClient
    {
        public const string DefaultBaseUrl = "https://api.vk.com/method/";
        private readonly string version = "5.92";


        public int errorCode { get; set; }
        public string errorMsg { get; set; }


        private readonly string _token;
        private readonly HttpClient _httpClient;

        private static string lpServerName = null;
        private static string lpServerKey = null;
        private static string lpServerTs = null;

        /// <summary>
        /// Occurs before sending a request to API
        /// </summary>
        public event EventHandler<ApiRequestEventArgs> MakingApiRequest;

        /// <summary>
        /// Occurs after receiving the response to an API request
        /// </summary>
        public event EventHandler<ApiResponseEventArgs> ApiResponseReceived;

        /// <summary>
        /// Occurs when exception was thrown on request
        /// </summary>
        public event EventHandler<ApiRequestExceptionEventArgs> ApiRequestException;

        public VkAttachmentManager _attachemntManager;


        public VKBotClient(string token, HttpClient httpClient = null)
        {
            _token = token;
            _httpClient = httpClient ?? new HttpClient();

            _attachemntManager = new VkAttachmentManager();
        }

        public Task<SendMessagesResponse> SendTextMessageAsync(
            string userId,
            string text,
            CancellationToken cancellationToken,
            string kbString = null,
            IEnumerable< AttachmentFile> files = null)
        {
            // Получить строку для Attachments
            // Добавить её в SendMessageRequest, а там реализовать её прописывание
            string attachmentsString = null;
            if (files != null)
            {
                var attachments = _attachemntManager.GetAttachments(files, userId).ConfigureAwait(false).GetAwaiter()
                    .GetResult();
                if (attachments.Count > 0)
                    //attachmentsString = JsonConvert.SerializeObject(attachments);
                    attachmentsString = string.Join(",", attachments);
            }

            var req = new SendMessageRequest(userId, text, kbString, attachmentsString);
            // 
            //var req = new SendMessageRequest(userId, WebUtility.UrlEncode(text), WebUtility.UrlEncode(kbString));            
            string url = $"{DefaultBaseUrl}{req.GetQuery()}&access_token={_token}&v={version}";
            //url += kbString != null ? $"&keyboard={WebUtility.UrlEncode(kbString)}" : "";            
            return MakeRequestAsync(req, url, cancellationToken);  
        }        
        
        public Task<GetUserInfoResponse> GetUserInfoAsync(
            string userId,
            CancellationToken cancellationToken)
        {
            var req = new GetUserInfoRequest(userId);
            string url = $"{DefaultBaseUrl}{req.GetQuery()}&access_token={_token}&v={version}";
            return MakeRequestAsync(req, url, cancellationToken);  
        }

        public async Task<TResponse> MakeRequestAsync<TResponse>(
            IRequest<TResponse> request,
             string url,
            CancellationToken cancellationToken)
        {

            var httpRequest = new HttpRequestMessage(request.Method, url)
            {
                Content = request.ToHttpContent()
            };

            var reqDataArgs = new ApiRequestEventArgs
            {
                MethodName = request.Method.ToString(),
                Url = url,
                HttpContent = url, // TODO content log
            };
            //  request.HttpContent.ReadAsStringAsync().Result
            MakingApiRequest?.Invoke(this, reqDataArgs);

            HttpResponseMessage httpResponse;
            try
            {
                try
                {
                    httpResponse = await _httpClient.SendAsync(httpRequest, cancellationToken)
                        .ConfigureAwait(false);
                }
                catch (TaskCanceledException e)
                {
                    if (cancellationToken.IsCancellationRequested)
                        throw;

                    throw new ApiRequestException("Request timed out", 408, e);
                }
            }
            catch (Exception e)
            {
                ApiRequestException?.Invoke(this, new ApiRequestExceptionEventArgs()
                {
                    ApiRequestEventArgs = reqDataArgs,
                    Exception = e
                });
                throw;
            }

            // required since user might be able to set new status code using following event arg
            var actualResponseStatusCode = httpResponse.StatusCode;
            string responseJson = await httpResponse.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            ApiResponseReceived?.Invoke(this, new ApiResponseEventArgs
            {
                ResponseMessage = httpResponse,
                ApiRequestEventArgs = reqDataArgs
            });

            switch (actualResponseStatusCode)
            {
                case HttpStatusCode.OK:
                    break;
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.BadRequest when !string.IsNullOrWhiteSpace(responseJson):
                case HttpStatusCode.Forbidden when !string.IsNullOrWhiteSpace(responseJson):
                case HttpStatusCode.Conflict when !string.IsNullOrWhiteSpace(responseJson):
                    // Do NOT throw here, an ApiRequestException will be thrown next
                    break;
                default:
                    httpResponse.EnsureSuccessStatusCode();
                    break;
            }

            var result = request.DeserializeResponse(responseJson);
            var rtt = new ApiResponse<TResponse> // ToDo is required? unit test
            {
                Ok = false,
                Description = "No response received"
            };

            ApiResponse<TResponse> apiResponse;
            if (result != null)
            {
                apiResponse = new ApiResponse<TResponse>
                {
                    Result = result,
                    Ok = true,
                };
            }
            else
            {
                apiResponse = new ApiResponse<TResponse> // ToDo is required? unit test
                {
                    Ok = false,
                    Description = "No response received"
                };
            }

            if (!apiResponse.Ok)
                throw ApiExceptionParser.Parse(apiResponse);

            return apiResponse.Result;
        }


        //public Message SendTextMessage(string msgText)
        //{
        //    return SendTextMessageAsync(msgText, CancellationToken.None).Result;
        //}

        //public async Task<Update[]> MakeUpdatesAsync(int groupId, CancellationToken cancellationToken)
        //{

        //    //    lpServerName = null;
        //    //private static string lpServerKey = null;
        //    //private static string lpServerSt = null;
        //    LongPollCommonInfoResponse preUpdates = null;

        //    var tryCount = 5;

        //    for (var i = 0; i< tryCount; i++)
        //    {
        //        preUpdates = await GetCommonInfo(groupId, cancellationToken);
        //        if (preUpdates.Ts != null)
        //            continue;

        //        Thread.Sleep(1000);
        //    }

        //    if (preUpdates == null)
        //        throw new Exception();

        //    if (preUpdates.Ts == null)
        //    {
        //        preUpdates = await GetCommonInfo(groupId, cancellationToken);
        //    }

        //    List<Update> listUpd = new List<Update>();
        //    foreach(var ret in preUpdates.Updates)
        //    {                
        //        //Update upd = JsonConvert.DeserializeObject<Update>(requestString);
        //        Update upd = MakeUpdate(ret); // JsonConvert.DeserializeObject<Update>(requestString);
        //        if (upd != null)
        //            listUpd.Add(upd);
        //    }
        //    return listUpd.ToArray();            
        //}

        public async Task<Update[]> MakeUpdatesAsync(int groupId, CancellationToken cancellationToken)
        {
            bool c1 = true;
            bool c2 = true;
            LongPollCommonInfoResponse preUpdates = null;
            int tryCount = 0;
            int maxCount = 15;

            while (c1)
            {
                if(tryCount > maxCount)
                {
                    //  В перспективе - ApiRequestException
                    throw new Exception($"Проблема получения данных от сервера - {tryCount}");
                }
                if(lpServerName == null || lpServerKey == null || lpServerTs == null)
                {
                    await GetServerParams(groupId, cancellationToken);
                    if (lpServerName == null || lpServerKey == null || lpServerTs == null)
                    {
                        Thread.Sleep(1000);                        
                        tryCount++;
                        continue;
                    }
                }

                while (c2)
                {
                    preUpdates = await GetCommonInfo(groupId, cancellationToken);
                    lpServerTs = preUpdates.Ts;
                    if (preUpdates.Error == null)
                    {
                        c1 = false;
                        c2 = false;
                    }
                    else
                    {
                        if (preUpdates.Error != 1)
                        {
                            lpServerName = null;
                            lpServerKey = null;
                            c2 = false;
                        }
                        Thread.Sleep(1000);
                        tryCount++;
                    }
                }
            }            

            
            List<Update> listUpd = new List<Update>();
            foreach (var ret in preUpdates.Updates)
            {
                //Update upd = JsonConvert.DeserializeObject<Update>(requestString);
                Update upd = MakeUpdate(ret); // JsonConvert.DeserializeObject<Update>(requestString);
                if (upd != null)
                    listUpd.Add(upd);
            }
            return listUpd.ToArray();
        }

        private async Task GetServerParams(int groupId, CancellationToken cancellationToken)
        {
            var req = new LongPollServerRequest(groupId);
            string url = $"{DefaultBaseUrl}{req.GetQuery()}&access_token={_token}&v={version}";
            var lpServer = await MakeRequestAsync(req, url, cancellationToken);
            lpServerName = lpServer.ServerInfo.server;
            lpServerKey = lpServer.ServerInfo.Key;
            lpServerTs = lpServer.ServerInfo.Ts;
        }

        private async Task<LongPollCommonInfoResponse> GetCommonInfo(int groupId, CancellationToken cancellationToken)
        {
            //var req = new LongPollServerRequest(groupId);
            //string url = $"{DefaultBaseUrl}{req.GetQuery()}&access_token={_token}&v={version}";
            //var lpServer = await MakeRequestAsync(req, url, cancellationToken);

            var url = $"{lpServerName}?act=a_check&key={lpServerKey}&ts={lpServerTs}&wait=25";

            var commonInfoResponse = new LongPollCommonInfoRequest();
            return await MakeRequestAsync(commonInfoResponse, url, cancellationToken);
        }

        private Update MakeUpdate(CallbackCommonResponse ret)
        {
            Update update = new Update();
            switch (ret.Type)
            {
                //case "message_allow"
                //case "message_deny"
                //case "confirmation":
                //    //< appSettings >
                //    //< add key = "VK_confirmation" value = "c30c7e45" />
                //    //</ appSettings >
                //    update = null;
                //    //  Добыть ответ из ... ?
                //    responseOk = ConfigurationManager.AppSettings["VK_confirmation"] ?? "c30c7e45";
                //    break;
                case "message_new":
                    var mess = JsonConvert.DeserializeObject<Message>(ret.Object.ToString());
                    update.Message = mess;
                    update.Id = mess.Id;
                    break;
                default:
                    update = null;
                    break;
            }
            return update;
        }

        //public async Task<string> SendGetRequest(/*string userIdOrPhone, */CancellationToken ct)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        string ss = $"{strCommand}access_token={_accessToken}&v={version}";
        //        if (strParam.Any())
        //        {
        //            ss += $"&{strParam}";
        //        }
        //        Uri uri = new Uri(_baseUri, ss);

        //        HttpResponseMessage response = await client.GetAsync(uri, ct);
        //        if (response.StatusCode != HttpStatusCode.OK)
        //        {
        //            errorCode = response.StatusCode.GetHashCode();
        //            errorMsg = response.StatusCode.ToString();
        //            throw new HttpRequestException($"GetProfile received status code {response.StatusCode.ToString()}");
        //        }
        //        string data = await response.Content.ReadAsStringAsync();

        //        return data;
        //    }
        //}

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

        private static class JsonFields
        {
            public const string Response = "response";
            public const string Error = "error";
            public const string Data = "data";
            public const string Message = "message";
            //public const string UserId = "user_id";

            public const string MessageId = "message_id";
        }

        public class ExecuteError
        {
            [JsonProperty("error_code")]
            public int ErrorCode { get; set; }
            [JsonProperty("error_msg")]
            public string ErrorMsg { get; set; }
            //[JsonProperty("request_params")]
            //public string[] RequestParams { get; set; }        
        }
    }
}
