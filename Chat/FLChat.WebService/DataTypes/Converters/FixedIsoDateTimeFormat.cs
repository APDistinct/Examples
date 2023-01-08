using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.DataTypes.Converters
{
    public class FixedIsoDateTimeConverter : IsoDateTimeConverter
    {
        public FixedIsoDateTimeConverter() {
            DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fff";
        }
    }
}
