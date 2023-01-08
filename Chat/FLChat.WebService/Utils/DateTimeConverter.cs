using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FLChat.WebService.Utils
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private readonly string _format;

        public DateTimeConverter(string format) {
            _format = format;
        }

        //public override bool CanConvert(Type objectType) {
        //    return (objectType == typeof(DateTime));
        //}

        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer) {
            DateTime date = (DateTime)value;
            //string format = "yyyy-MM-dd HH:mm:ss";
            //if (date.Hour == 0 &&
            //    date.Minute == 0 &&
            //    date.Second == 0 &&
            //    date.Millisecond == 0) {
            //    format = "yyyy-MM-dd";
            //}
            writer.WriteValue(date.ToString(_format));
        }

        public override bool CanRead => true;

        //public override DateTime ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        //    string dt = (string)reader.Value;
        //    return DateTime.ParseExact(dt, _format, CultureInfo.InvariantCulture);
        //}

        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer) {
            string dt = (string)reader.Value;
            return DateTime.ParseExact(dt, _format, CultureInfo.InvariantCulture);
        }
    }
}
