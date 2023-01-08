using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Devino.Args;
using Devino.Exceptions;
using Devino.Logger;
using FLChat.Devino.Request;

namespace FLChat.Devino
{
    public abstract class AbstractHttpService
    {
        private string Scheme = "https";
        protected string Host = "integrationapi.net";

        public IDevinoLogger Logger { get; }

        protected string Method { get; set; }

        protected HttpClient HttpClient { get; } = new HttpClient();

        protected AbstractHttpService(IDevinoLogger logger)
        {
            Logger = logger;
        }

        private string GetUri(string query)
        {
            var result = $"{Scheme}://{Host}";
            if (!string.IsNullOrWhiteSpace(Method))
                result = $"{result}/{Method}";
            if (!string.IsNullOrWhiteSpace(query))
                result = $"{result}/{query}";
            return result;
        }

        public async Task<TResponse> MakeRequestAsync<TResponse>(
            IRequest<TResponse> request,
            CancellationToken cancellationToken)
        {
            string uri = GetUri(request.GetQuery());

            var httpRequest = new HttpRequestMessage(request.Method, uri);

            if (request.Method == HttpMethod.Post)
                httpRequest.Content = request.ToHttpContent();

            var requestArgs = new ApiRequestEventArgs
            {
                MethodName = request.Method.ToString(),
                Url = uri,
                HttpContent = httpRequest.Content,
            };

            Logger.LogApiRequest(requestArgs);

            HttpResponseMessage httpResponse;
            try
            {
                httpResponse = await HttpClient.SendAsync(httpRequest, cancellationToken);
            }
            catch (TaskCanceledException /*e*/)
            {
                if (cancellationToken.IsCancellationRequested)
                    throw;

                throw new Exception("Request timed out");
            }
            catch (Exception e)
            {
                var exceptionEventArgs = new ApiRequestExceptionEventArgs
                {
                    Exception = e,
                    ApiRequestEventArgs = requestArgs
                };

                Logger.LogApiRequestException(exceptionEventArgs);
                throw;
            }

            var responseEventArgs = new ApiResponseEventArgs()
            {
                ResponseMessage = httpResponse,
                ApiRequestEventArgs = requestArgs
            };
            Logger.LogApiResponse(responseEventArgs);

            string responseJson = await httpResponse.Content.ReadAsStringAsync();

            switch (httpResponse.StatusCode)
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

            TResponse result = request.DeserializeResponse(responseJson);
            return result;
        }
    }
}
