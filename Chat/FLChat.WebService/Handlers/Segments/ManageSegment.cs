using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.Handlers.Segments
{
    /// <summary>
    /// Return list of not deleted segments names
    /// </summary>
    public class ManageSegment : IObjectedHandlerStrategy<JObject, object>
    {
        public bool IsReusable => true;
        public const string Key = "id";

        public object ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, JObject input)
        {
            Segment segment;
            string sinput = (string)input[Key];
            if (Guid.TryParse(sinput, out Guid sid))
                segment = entities.Segment.Where(x => x.Id == sid).FirstOrDefault();
            else
                segment = entities.Segment.Where(x => x.Name == sinput).FirstOrDefault();

            if (segment == null)
                throw new ErrorResponseException(
                    (int)HttpStatusCode.NotFound,
                    new ErrorResponse(ErrorResponse.Kind.user_not_found, $"Segment {input} not found"));

            var lists = JsonConvert.DeserializeObject < SegmentManageRequest >(input.ToString());

            /*return*/ ProcessRequest(entities, segment, lists);
            return null;
        }

        private void ProcessRequest(ChatEntities entities, Segment segment, SegmentManageRequest segmentMR)
        {
            var listAdd = segmentMR.Add;
            var listRemove = segmentMR.Remove;

            AddList(entities, segment, listAdd);
            RemoveList(entities, segment, listRemove);            
        }

        private void AddList(ChatEntities entities, Segment segment, IEnumerable<Guid> list)
        {
            foreach (var mem in list)
            {
                var user = entities.User.Where(x => x.Id == mem).FirstOrDefault();
                if (user != null)
                {
                    if(!segment.Members.Contains(user))
                        segment.Members.Add(user);
                }                    
            }
            entities.SaveChanges();
        }

        private void RemoveList(ChatEntities entities, Segment segment, IEnumerable<Guid> list)
        {
            foreach (var mem in list)
            {
                var user = entities.User.Where(x => x.Id == mem).FirstOrDefault();
                if(user != null)
                    segment.Members.Remove(user);
            }
            entities.SaveChanges();
        }
    }
}