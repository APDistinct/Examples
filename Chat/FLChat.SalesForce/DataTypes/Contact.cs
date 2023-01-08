using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FLChat.SalesForce.DataTypes
{
    /// <summary>
    /// SalesForce data type for Contact (User in FLChat database)
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(DefaultNamingStrategy))]
    public class Contact
    {
        [JsonProperty(Required = Required.Always)]
        public string Id { get; set; }

        public bool IsDeleted { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime? Birthdate { get; set; }

        [JsonProperty(PropertyName = "BonusPoints__c")]
        public double? BonusPoints {get; set;}

        public DateTime CreatedDate { get; set; }

        [JsonProperty(PropertyName = "EmailPermission__c")]
        public bool? EmailPermission { get; set; }

        [JsonProperty(PropertyName = "LastOrderDate__c")]
        public DateTime? LastOrderDate { get; set; }

        [JsonProperty(PropertyName = "MailingCity")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "MailingCountry")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "MailingState")]
        public string Region { get; set; }

        [JsonProperty("MobilePhone")]
        public string Phone { get; set; }

        public string PhotoUrl { get; set; }

        public string ReportsToId { get; set; }

        [JsonProperty(PropertyName = "SMSPermission__c")]
        public bool? SMSPermission { get; set; }

        public string Title { get; set; }

        [JsonProperty(PropertyName = "Segment__c")]
        public string Segments { get; set; }
    }
}
