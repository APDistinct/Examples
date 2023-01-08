using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace RestApiClient
{
    /// <summary>
    /// Клиент для взаимодействия с Devino REST API
    /// </summary>
    public class RestService
    {
        private const string Host = "integrationapi.net/rest";
        private const string Scheme = "https";

        

        public string Post(string path, string queryString)
        {
            var uriBuilder = new UriBuilder { Host = Host, Path = path, Scheme = Scheme };

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uriBuilder.Uri);

            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            if (httpWebRequest.Proxy != null)
            {
                httpWebRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;
            }

            byte[] byteArr = Encoding.UTF8.GetBytes(queryString);
            httpWebRequest.ContentLength = byteArr.Length;

            try
            {
                using (var stream = httpWebRequest.GetRequestStream())
                {
                    stream.Write(byteArr, 0, byteArr.Length);
                }

                var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var responseStream = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8))
                {
                    return responseStream.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    var contentStream = ex.Response.GetResponseStream();
                    if (contentStream != null)
                    {
                        using (var reader = new StreamReader(contentStream))
                        {
                            var content = reader.ReadToEnd();
                            if (!string.IsNullOrEmpty(content))
                                throw new RestApiException(JsonConvert.DeserializeObject<ErrorResult>(content));
                            throw;
                        }
                    }
                }
                throw;
            }
        }

        public string Get(string path, string queryString)
        {
            var uriBuilder = new UriBuilder { Host = Host, Path = path, Query = queryString, Scheme = Scheme };

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uriBuilder.Uri);

            httpWebRequest.Method = "GET";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            if (httpWebRequest.Proxy != null)
            {
                httpWebRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;
            }

            try
            {
                var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var responseStream = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8))
                {
                    return responseStream.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    var contentStream = ex.Response.GetResponseStream();
                    if (contentStream != null)
                    {
                        using (var reader = new StreamReader(contentStream))
                        {
                            var content = reader.ReadToEnd();
                            if (!string.IsNullOrEmpty(content))
                                throw new RestApiException(JsonConvert.DeserializeObject<ErrorResult>(content));
                        }
                    }
                }
                throw;
            }
        }
    }
}