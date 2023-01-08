using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

using SalesForce.Types;
using SalesForce.Requests;
using System.Threading;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SalesForce.TestApp
{
    class Program
    {
        static void Main(string[] args) {
            try {
                string authUrl = ConfigurationManager.AppSettings["auth_url"] ?? throw new ArgumentNullException("auth_url");
                string clientId = ConfigurationManager.AppSettings["client_id"] ?? throw new ArgumentNullException("client_id");
                string secret = ConfigurationManager.AppSettings["secret"] ?? throw new ArgumentNullException("secret");
                string username = ConfigurationManager.AppSettings["username"] ?? throw new ArgumentNullException("username");
                string password = ConfigurationManager.AppSettings["password"] ?? throw new ArgumentNullException("password");
                string query = ConfigurationManager.AppSettings["query"] ?? throw new ArgumentNullException("query");

                string token;
                string instance_url;

                using (SalesForceClient cl = new SalesForceClient(authUrl)) {
                    cl.MakingApiRequest += Cl_MakingApiRequest;
                    cl.ApiResponseReceived += Cl_ApiResponseReceived;
                    cl.ApiRequestException += Cl_ApiRequestException;
                    Task<AuthResponse> task = cl.Auth(clientId, secret, username, password, CancellationToken.None);
                    task.Wait();
                    AuthResponse resp = task.Result;
                    token = resp.AccessToken;
                    instance_url = resp.InstanceUrl;
                    Console.WriteLine("token:" + resp.AccessToken);
                    Console.WriteLine("url: " + resp.InstanceUrl);
                }

                using (SalesForceClient cl = new SalesForceClient(instance_url, token)) {
                    cl.MakingApiRequest += Cl_MakingApiRequest;
                    cl.ApiResponseReceived += Cl_ApiResponseReceived;
                    cl.ApiRequestException += Cl_ApiRequestException;

                    Task<QueryResponse<JObject>> task = cl.Query<JObject>(query, CancellationToken.None);
                    task.Wait();
                    if (task.Result.Done) {
                        Console.WriteLine($"total: {task.Result.TotalSize.ToString()}");
                        string json = JsonConvert.SerializeObject(task.Result.Records);
                        File.WriteAllText("result.json", json);
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
            Console.ReadLine();
        }

        private static void Cl_ApiRequestException(object sender, Args.ApiRequestExceptionEventArgs e) {
            File.WriteAllText("exception.log", e.Exception.ToString());
        }

        private static void Cl_ApiResponseReceived(object sender, Args.ApiResponseEventArgs e) {
            File.WriteAllText("response.log", e.ResponseMessage.ToString());
        }

        private static void Cl_MakingApiRequest(object sender, Args.ApiRequestEventArgs e) {
            File.WriteAllText("request.log", e.HttpContent?.ToString() ?? "");
        }
    }
}
