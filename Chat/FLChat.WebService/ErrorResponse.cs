using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using FLChat.WebService.DataTypes;

namespace FLChat.WebService
{
    /// <summary>
    /// Service error response
    /// </summary>
    public class ErrorResponse
    {
        public enum Kind
        {
            url_not_found,
            missed_auth_token,
            invalid_auth_token,
            input_data_error,
            uri_key_error,
            not_support,
            not_found,
            user_not_found,
            expired,
            error,
            max_size_limit,
            bad_file_type,
            access_denied,
            exceed_limit,
            wrong_old_password,
            invalid_code
        }

        public ErrorResponse(Kind error, string descr, string trace) {
            Error = error;
            Descr = descr;
            Trace = trace;
        }

        public ErrorResponse(Kind error, string descr) : this(error, descr, null) {
        }

        public ErrorResponse(Kind error, Exception e) : this(error, e.Message, e.ToString()) {
        }

        public ErrorResponse(Exception e) : this(Kind.error, e.Message, e.ToString()) {
        }

        [JsonProperty(PropertyName = "error")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Kind Error { get; }

        [JsonProperty(PropertyName = "error_descr")]
        public string Descr { get; }

        [JsonProperty(PropertyName = "error_trace", NullValueHandling = NullValueHandling.Ignore)]
        public string Trace { get; }
    }

    //[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class MessageLimitErrorResponse : ErrorResponse
    {
        public MessageLimitErrorResponse(LimitInfo limit) : base(Kind.exceed_limit, "Limit was exceeded") {
            Limit = limit;
        }

        [JsonProperty(PropertyName = "limit")]
        public LimitInfo Limit { get; }
    }
}
