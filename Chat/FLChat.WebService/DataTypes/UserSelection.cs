using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using FLChat.DAL.Model;

namespace FLChat.WebService.DataTypes
{
    /// <summary>
    /// User selections
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class UserSelection
    {
        public enum SelectionType { Deep, Shallow }

        [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public class UserStructureSelection
        {
            public Guid UserId { get; set; }

            [JsonConverter(typeof(StringEnumConverter), typeof(SnakeCaseNamingStrategy))]
            public SelectionType Type { get; set; }
        }

        /// <summary>
        /// Selected user's with his structure
        /// </summary>
        public List<UserStructureSelection> IncludeWithStructure { get; set; }

        /// <summary>
        /// Users, excluded from selection with his structure
        /// </summary>
        public List<Guid> ExcludeWithStructure { get; set; }

        /// <summary>
        /// Exluded users
        /// </summary>
        public List<Guid> Exclude { get; set; }

        /// <summary>
        /// Include users
        /// </summary>
        public List<Guid> Include { get; set; }

        /// <summary>
        /// List of segments, can include segment Id or structure id like 'seg-...'
        /// </summary>
        public List<string> Segments { get; set; }

        /// <summary>
        /// Is exists any selected user
        /// </summary>
        [JsonIgnore]
        public bool IsExists => IncludeWithStructure?.Count > 0 || Include?.Count > 0 || Segments?.Count > 0;
    }

    public static class UserSelectionExtentions
    {
        /// <summary>
        /// Convert IEnumerable<UserSelection.UserStructureSelection> to IEnumerable<Tuple<Guid, int?>>
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<Guid, int?>> DeepEnumToInt(this IEnumerable<UserSelection.UserStructureSelection> list)
            => list?.Select(i => Tuple.Create(i.UserId, i.Type == UserSelection.SelectionType.Deep ? (int?)null : 1));

        /// <summary>
        /// Convert list of segments id to guids
        /// </summary>
        /// <param name="segments"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static IEnumerable<Guid> ToSegmentGuids(this IEnumerable<string> segments) {
            if (segments == null)
                return null;
            return segments
                .Select(s => s.StartsWith(Segment.Prefix) ? s.Substring(Segment.Prefix.Length) : s)
                .Select(s => Guid.Parse(s));
        }

        /// <summary>
        /// Convert web service's user selection object to ChatEntities' object
        /// </summary>
        /// <param name="selection">web service's object</param>
        /// <returns>ChatEntities' object</returns>
        public static DAL.DataTypes.UserSelection Convert(this UserSelection selection) {
            return new DAL.DataTypes.UserSelection() {
                IncludeWithStructure = selection.IncludeWithStructure.DeepEnumToInt(),
                ExcludeWithStructure = selection.ExcludeWithStructure,
                Include = selection.Include,
                Exclude = selection.Exclude,
                Segments = selection.Segments.ToSegmentGuids(),
            };
        }
    }
}
