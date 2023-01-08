using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

using FLChat.DAL.Model;

namespace FLChat.WebService.DataTypes
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class LocationInfo
    {
        private readonly City _city;

        public LocationInfo(City city, string zip) {
            _city = city;
            Zip = zip;
        }

        public string Zip { get; }
        public string City => _city.Name;
        public string Region => _city.Region.Name;
        public string Country => _city.Region.Country.Name;
    }
}
