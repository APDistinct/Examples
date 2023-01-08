using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Http;
using System.Web;

using Newtonsoft.Json;

using SalesForce.Args;
using SalesForce.Exceptions;
using SalesForce.Requests;
using SalesForce.Types;

namespace SalesForce
{
    public class SalesForceClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;

        public class RequestArgs
        {
            public string HttpMethod { get; set; }
            public string Url { get; set; }
            public string Request { get; set; }
            public HttpContent Content { get; set; }
            public int? StatusCode { get; set; }
            public string Response { get; set; }
            public Exception Exception { get; set; }
        }

        #region events
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
        #endregion

        /// <summary>
        /// Timeout for requests
        /// </summary>
        public TimeSpan Timeout {
            get => _httpClient.Timeout;
            set => _httpClient.Timeout = value;
        }

        public string BaseUrl { get; }

        public SalesForceClient(string baseUrl, string token = null) {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _httpClient = new HttpClient();            
            BaseUrl = baseUrl;
            _token = token;
        }

        /// <summary>
        /// Perform auth request and return new SalesForceClient within auth token
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="secret"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="ct"></param>
        /// <param name="authUrl"></param>
        /// <returns></returns>
        public static async Task<SalesForceClient> CreateAndAuth(            
            string clientId, string secret, string userName, string password,
            CancellationToken ct,
            string authUrl = @"https://login.salesforce.com") {
            using (SalesForceClient cl = new SalesForceClient(authUrl)) {
                AuthResponse resp =  await cl.Auth(clientId, secret, userName, password, CancellationToken.None);

                string token = resp.AccessToken;
                string instance_url = resp.InstanceUrl;
                return new SalesForceClient(instance_url, token);
            }
        }

        /// <summary>
        /// Make auth request
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="secret"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<AuthResponse> Auth(string clientId, string secret, string userName, string password,
            CancellationToken ct) => await MakeRequestAsync(new AuthByPasswordRequest(
                clientId, secret, userName, password), ct);

        /// <summary>
        /// Execute a SOQL Query
        /// </summary>
        /// <typeparam name="TRecord">type of result object</typeparam>
        /// <param name="query">SOQL query</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<QueryResponse<TRecord>> Query<TRecord>(string query, CancellationToken ct) where TRecord : class
            => await MakeRequestAsync(new QueryRequest<TRecord>(query), ct);

        /// <summary>
        /// Retrieving the Remaining SOQL Query Results
        /// </summary>
        /// <typeparam name="TRecord"></typeparam>
        /// <param name="url">field nextRecordsUrl in Execute SOQL Query response</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<QueryResponse<TRecord>> QueryNext<TRecord>(string url, CancellationToken ct) where TRecord : class
            => await MakeRequestAsync(new QueryNextRequest<TRecord>(url), ct);

        public async Task<TResponse> MakeRequestAsync<TRequest, TResponse>(
            IRequest<TRequest, TResponse> request,
            CancellationToken cancellationToken) 
            where TRequest : class 
            where TResponse : class {

            string url = BaseUrl + request.MethodName;
            if (request.QueryParams != null) {
                var query = HttpUtility.ParseQueryString(string.Empty);
                foreach (var item in request.QueryParams)
                    query[item.Key] = item.Value;
                url = string.Concat(url, "?", query.ToString());
            }
                                    
            string requestData;
            if (request.RequestBody != null)
                requestData = JsonConvert.SerializeObject(request.RequestBody);
            else
                requestData = string.Empty;

            HttpRequestMessage httpRequest = new HttpRequestMessage(request.Method, url) {
                Content = request.Method != HttpMethod.Get 
                    ? new StringContent(requestData, Encoding.UTF8, "application/json") 
                    : null
            };
            httpRequest.Headers.Add("Authorization", string.Concat("Bearer ", _token));

            var reqDataArgs = new ApiRequestEventArgs {
                MethodName = request.MethodName,
                HttpContent = httpRequest.Content,
            };
            MakingApiRequest?.Invoke(this, reqDataArgs);

            HttpResponseMessage httpResponse;
            try {
                try {
                    httpResponse = await _httpClient.SendAsync(httpRequest, cancellationToken)
                        .ConfigureAwait(false);
                } catch (TaskCanceledException e) {
                    if (cancellationToken.IsCancellationRequested)
                        throw;

                    throw new ApiRequestException("Request timed out", e);
                }
            } catch (Exception e) {
                ApiRequestException?.Invoke(this, new ApiRequestExceptionEventArgs() {
                    ApiRequestEventArgs = reqDataArgs,
                    Exception = e
                });
                throw;
            }

            // required since user might be able to set new status code using following event arg
            var actualResponseStatusCode = httpResponse.StatusCode;

            string responseJson = await httpResponse.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            ApiResponseReceived?.Invoke(this, new ApiResponseEventArgs {
                ResponseMessage = httpResponse,
                ApiRequestEventArgs = reqDataArgs
            });

            if (httpResponse.StatusCode == HttpStatusCode.OK) {
                return JsonConvert.DeserializeObject<TResponse>(responseJson);
            } else {
                ErrorResponse err = null;
                try {
                    err = JsonConvert.DeserializeObject<ErrorResponse>(responseJson);                    
                } catch { }
                if (err != null)
                    throw new ApiRequestException(httpResponse.StatusCode, err);
                else
                    throw new ApiRequestException(httpResponse.StatusCode, responseJson);
            }
        }       

        public void Dispose() {
            _httpClient.Dispose();
        }
    }
}
