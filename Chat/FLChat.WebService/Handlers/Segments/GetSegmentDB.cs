using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.WebService.Utils;

namespace FLChat.WebService.Handlers.Segments
{
    /// <summary>
    /// Return list of not deleted segments names
    /// </summary>
    public class GetSegmentDB : IObjectedHandlerStrategy<SegmentDBRequest, SegmentDBResponse>
    {
        public bool IsReusable => true;
        public int MaxCount { get; set; } = 100;
        public static readonly string Key = typeof(SegmentDBRequest).GetJsonPropertyName(nameof(SegmentDBRequest.Segment));

        public SegmentDBResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, SegmentDBRequest input)
        {
            Segment segment;
            string sinput = input.Segment;

            if (Guid.TryParse(sinput, out Guid sid))
                segment = entities.Segment.Where(x => x.Id == sid).FirstOrDefault();
            else
                segment = entities.Segment.Where(x => x.Name == sinput).FirstOrDefault();

            if (segment == null)
                throw new ErrorResponseException(
                    (int)HttpStatusCode.NotFound,
                    new ErrorResponse(ErrorResponse.Kind.user_not_found, $"Segment {sinput} not found"));
            input.MaxCount = MaxCount;
            return ProcessRequest(entities, currUserInfo.UserId, segment, input);
        }

        private SegmentDBResponse ProcessRequest(ChatEntities entities, Guid currUserId, Segment segment, SegmentDBRequest partial)
        {
            int? totalCount = null;
            var slist = segment.Members.Select(x => x.Id).ToList();
            var list = entities.User.Where(x => slist.Contains(x.Id));                
            if (partial.SearchValue != null && partial.SearchValue.Length >= 3)
                list = list.FindString(partial.SearchValue);
            var olist = list
                .OrderBy(z => z.FullName)
                .TakePart(partial)
                .ToList();
            if ((partial.Offset ?? 0) == 0)
                totalCount = segment.Members.Count;
            return new SegmentDBResponse(segment, olist.Count, olist, partial)
            {
                TotalCount = totalCount
            }; 
        }
    }
}