using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.WebService.Utils;

namespace FLChat.WebService.Handlers.User
{
    public class SetUserTransport : IObjectedHandlerStrategy<TransportInfoResponse, TransportInfoResponse>
    {
        public static readonly string Key = typeof(TransportInfoResponse).GetJsonPropertyName(nameof(TransportInfoResponse.Id));

        public bool IsReusable => true;

        public TransportInfoResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, TransportInfoResponse input) {
            if (!Guid.TryParse(input.Id, out Guid userId))
                throw new ErrorResponseException(HttpStatusCode.NotFound, ErrorResponse.Kind.user_not_found, $"user with id [{input}] has not found");

            DAL.Model.User user = entities.User.Where(u => u.Id == userId && u.Enabled).Include(u => u.Transports).FirstOrDefault();
            if (user == null)
                throw new ErrorResponseException(HttpStatusCode.NotFound, ErrorResponse.Kind.user_not_found, $"user with id [{input}] has not found");

            foreach (TransportInfo ti in input.Transports) {
                DAL.Model.Transport t = user.Transports.Get(ti.Kind);
                if (t == null) {
                    if (ti.Kind != TransportKind.FLChat)
                        throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, $"Can't create {ti.Kind.ToString()} transport (only for FLChat)");
                    t = new DAL.Model.Transport();
                    user.Transports.Add(t);
                }
                if (t.Enabled != ti.Enabled)
                    t.Enabled = ti.Enabled;
                if (t.TransportOuterId != ti.TransportId)
                    t.TransportOuterId = ti.TransportId;
            }
            entities.SaveChanges();
            return new TransportInfoResponse(user);
        }
    }
}
