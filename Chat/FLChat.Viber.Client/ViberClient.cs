using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Http;

using Newtonsoft.Json;

using FLChat.Viber.Client.Args;
using FLChat.Viber.Client.Exceptions;
using FLChat.Viber.Client.Requests;
using FLChat.Viber.Client.Types;

namespace FLChat.Viber.Client
{
    public class ViberClient : IDisposable
    {
        public const string BaseUrl = "https://chatapi.viber.com/pa/";
        public const string HeaderAuthToken = "X-Viber-Auth-Token";
        public const int DefaultMinApiVersion = 3;

        private readonly HttpClient _httpClient;
        private readonly string _token;

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


        public ViberClient(string token) {
            _httpClient = new HttpClient();
            //_httpClient.DefaultRequestHeaders.Add(HeaderAuthToken, token);
            _token = token;
        }

        public async Task<TResponse> MakeRequestAsync<TResponse>(
            IRequest<TResponse> request,
            CancellationToken cancellationToken) where TResponse : ResponseBase {
            string url = BaseUrl + request.MethodName;

            var httpRequest = new HttpRequestMessage(request.Method, url) {
                Content = request.ToHttpContent()
            };
            httpRequest.Headers.Add(HeaderAuthToken, _token);

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

                    throw new ApiRequestException("Request timed out", 408, e);
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

            httpResponse.EnsureSuccessStatusCode();

            TResponse apiResponse = JsonConvert.DeserializeObject<TResponse>(responseJson);
            apiResponse.EnsureIsOk();

            return apiResponse;
        }

        public async Task<GetUserDetailsResponse> GetUserDetails(string receiver, CancellationToken ct)
            => await this.MakeRequestAsync(new GetUserDetailsRequest(receiver), ct);

        public async Task<SendMessageResponse> SendTextMessage(Sender sender, string receiver, string text, CancellationToken ct,
            Keyboard keyboard = null)
            => await MakeRequestAsync(new SendTextMessageRequest(sender, receiver, text) {
                Keyboard = keyboard,
                MinApiVersion = DefaultMinApiVersion,
            }, ct);

        public async Task<SendMessageResponse> SendPictureMesage(Sender sender, string receiver, string text, string media,
            CancellationToken ct,
            string thumbnail = null, Keyboard keyboard = null
            )
            => await MakeRequestAsync(new SendPictureMessageRequest(sender, receiver, text, media) {
                Thumbnail = thumbnail,
                Keyboard = keyboard,
                MinApiVersion = DefaultMinApiVersion,
            }, ct);

        public async Task<SendMessageResponse> SendFileMessage(Sender sender, string receiver,
            string media, int size, string fileName,
            CancellationToken ct,
            Keyboard keyboard = null)
            => await MakeRequestAsync(new SendFileMessageRequest(sender, receiver, media, size, fileName) {
                Keyboard = keyboard,
                MinApiVersion = DefaultMinApiVersion,
            }, ct);

        public async Task<SendMessageResponse> SendUrlMessage(Sender sender, string receiver, string media,
           CancellationToken ct,
           Keyboard keyboard = null)
           => await MakeRequestAsync(new SendUrlMessageRequest(sender, receiver, media) {
               Keyboard = keyboard,
               MinApiVersion = DefaultMinApiVersion,
           }, ct);

        public void Dispose() {
            _httpClient.Dispose();
        }
    }
}
