using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

namespace Devino.Viber
{
    public class GetStatusResponse
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
        [JsonProperty(PropertyName = "messages")]
        public IEnumerable<StatusResponse> Messages { get; set; }
    }

    public class StatusResponse
    {
        [JsonProperty(PropertyName = "providerId", Required = Required.Always)]
        public string ProviderId { get; set; }
        [JsonProperty(PropertyName = "code", Required = Required.Always)]
        public string Code { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "status")]
        public ViberStatus Status { get; set; }
        [JsonProperty(PropertyName = "statusAt")]
        public DateTime StatusAt { get; set; }
        [JsonProperty(PropertyName = "errorCode")]
        public string ErrorCode { get; set; }
        [JsonProperty(PropertyName = "smsStates")]
        public List<SmsState> SmsStates { get; set; }
    }

    public class SmsState
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "status")]
        public SmsStatus Status { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SmsStatus
    {
        [EnumMember(Value = "enqueued")]
        Enqueued,
        [EnumMember(Value = "sent")]
        Sent,
        [EnumMember(Value = "delivered")]
        Delivered,
        [EnumMember(Value = "undelivered")]
        Undelivered,        
    }

    //  Статусы СМС
    //enqueued – сообщение находится в очереди на отправку.
    //    sent – сообщение отправлено абоненту 
    //    delivered – сообщение доставлено абоненту.
    //    undelivered – сообщение отправлено, но не доставлено абоненту.

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ViberStatus
    {
        [EnumMember(Value = "enqueued")]
        Enqueued,
        [EnumMember(Value = "sent")]
        Sent,
        [EnumMember(Value = "delivered")]
        Delivered,
        [EnumMember(Value = "undelivered")]
        Undelivered,
        [EnumMember(Value = "read")]
        Read,
        [EnumMember(Value = "visited")]
        Visited,
        [EnumMember(Value = "failed")]
        Failed,
        [EnumMember(Value = "cancelled")]
        Cancelled,
        [EnumMember(Value = "vp_expired")]
        VpExpired,
        [EnumMember(Value = "unknown")]
        Unknown = -100,
    }

    //  Статусы вайбер
    //enqueued – сообщение находится в очереди на отправку. 
    //sent – сообщение отправлено абоненту 
    //delivered – сообщение доставлено абоненту. 
    //read – сообщение просмотрено абонентом. 
    //visited абонент перешел по ссылке в сообщении. 
    //undelivered – сообщение отправлено, но не доставлено абоненту. 
    //failed – сообщение не было отправлено в результат сбоя. 
    //cancelled –отправка сообщения отменена. 
    //vp_expired – сообщение просрочено, финальный статус не получен в рамках заданного validity period	
}
