using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Devino.Exceptions;
using FLChat.Devino.Request;
using Newtonsoft.Json;

namespace FLChat.Devino.SMS.Request
{
    //public class SendBulkRequest : RequestBase<SendBulkResponse>
    public class SendBulkRequest : RequestBase<List<string>>
    {
        public List<string> DestinationAddresses { get; } = new List<string>();

        public SendBulkRequest() : base("Sms/SendBulk", HttpMethod.Post)
        {
        }

        public override HttpContent ToHttpContent()
        {
            var values = DestinationAddresses.Select(a => new KeyValuePair<string, string>("destinationAddresses", a));
            var content = new FormUrlEncodedContent(values);
            //var message = string.Join(";", DestinationAddresses);
            //var content = new MultipartFormDataContent {{new StringContent(message, Encoding.UTF8, "multipart/form-data"), "destinationAddresses"}};
            return content;
        }

        public override List<string> DeserializeResponse(string responseJson)
        //public override SendBulkResponse DeserializeResponse(string responseJson)
        {
            if (responseJson.Contains("\"Code\":"))
            {
                var res = JsonConvert.DeserializeObject<BadResponse>(responseJson);
                throw new BadResponseException($"Code = {res.Code} \n {res.Description}");
            }

            var result = JsonConvert.DeserializeObject<List<string>>(responseJson);
            return result; //new SendBulkResponse { Result = result };
        }
    }
}
