using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.Handlers.Segments
{
    public class GetSegmentsAll : IObjectedHandlerStrategy<JObject, SegmentListResponse>
    {
        public bool IsReusable => true;

        public const string KeyFieldName = "include_empty";

        public SegmentListResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, JObject input) {
            Guid id = currUserInfo.UserId;
            var slist = entities.Segment.Where(u => !u.IsDeleted)
                .ToArray();

            List<SegmentInfo> list = new List<SegmentInfo>();
            if (slist != null) {
                foreach (var x in slist) {
                    list.Add(new SegmentInfo(x, SegCount(entities, id, x)));
                }
                if (!NeedGetAll(input)) {
                    list = list.Where(z => z.Count != 0).ToList();
                }
            }
            SegmentListResponse scr = new SegmentListResponse { Segments = list };
            return scr;
        }

        private int SegCount(ChatEntities entities, Guid id, Segment x) {
            int ret = x.GetMembersForUser(entities, id).Count;
            return ret;
        }
        private bool NeedGetAll(JObject jsonObject) {
            bool ret = false;
            if (jsonObject != null && jsonObject.Properties().Select(x => x.Name).Contains(KeyFieldName)) {
                ret = (bool)jsonObject[KeyFieldName];
            }
            return ret;
        }
    }
}
