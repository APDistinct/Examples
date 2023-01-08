using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;

namespace FLChat.WebService.Handlers.Segments
{
    /// <summary>
    /// List of all segments
    /// </summary>
    public class SegmentsList : IObjectedHandlerStrategy<object, SegmentListResponse>
    {
        public bool IsReusable => true;

        public SegmentListResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, object input) {
            var list = entities
                .Segment
                .OrderByDescending(s => s.IsDeleted)
                .ThenBy(s => s.Name)
                .ToArray()
                .Select(s => new SegmentInfo(s))
                .ToArray();
            return new SegmentListResponse { Segments = list };
        }
    }
}
