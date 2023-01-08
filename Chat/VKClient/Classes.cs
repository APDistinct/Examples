using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKAccess
{
    public class Info
    {
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("https_required")]
        public int HttpsRequired { get; set; }
        [JsonProperty("2fa_required")]
        public int TwoFaRequired { get; set; }
        [JsonProperty("own_posts_default")]
        public int OwnPostsDefault { get; set; }
        [JsonProperty("no_wall_replies")]
        public int NoWallReplies { get; set; }
        [JsonProperty("intro")]
        public int Intro { get; set; }
        [JsonProperty("lang")]
        public int Lang { get; set; }
    }
    //public class Message
    //{
    //    [JsonProperty("country")]
    //    public string Country { get; set; }
    //    [JsonProperty("https_required")]
    //    public int HttpsRequired { get; set; }
    //    [JsonProperty("2fa_required")]
    //    public int TwoFaRequired { get; set; }
    //    [JsonProperty("own_posts_default")]
    //    public int OwnPostsDefault { get; set; }
    //    [JsonProperty("no_wall_replies")]
    //    public int NoWallReplies { get; set; }
    //    [JsonProperty("intro")]
    //    public int Intro { get; set; }
    //    [JsonProperty("lang")]
    //    public int Lang { get; set; }
    //}

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
