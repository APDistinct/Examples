using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

using FLChat.PDAL.Model;
using FLChat.DAL;

namespace FLChat.PDAL
{
    public class RequestResponseLogWritter
    {
        IDictionary<object, long> _data = new Dictionary<object, long>();
        private object _lock = new object();

        private readonly bool _outcome;
        private readonly int TransportTypeId;

        public RequestResponseLogWritter(bool outcome, TransportKind kind) {
            _outcome = outcome;
            TransportTypeId = (int)kind;
        }

        public void Request(object reqObj, string url, string method, string request) {
            TransportLog log = null;
            using (ProtEntities entities = new ProtEntities()) {
                log = entities.TransportLog.Add(new TransportLog() {
                    TransportTypeId = TransportTypeId,
                    Url = url,
                    Method = method,
                    Outcome = _outcome,
                    Request = request,
                    Response = null,
                    StatusCode = null,
                    Exception = null
                });
                entities.SaveChanges();
            }

            lock (_lock) {
                _data[reqObj] = log.Id;
            }
        }

        public void Response(object reqObj, int statusCode, string response) {
            bool hasId = GetRecordId(reqObj, out long id);

            using (ProtEntities entities = new ProtEntities()) {
                if (hasId) {
                    entities.TransportLog.Where(l => l.Id == id)
                        .Update(l => new TransportLog() {
                            StatusCode = statusCode,
                            Response = response
                        });
                    //entities.SaveChanges();
                } else {
                    entities.TransportLog.Add(new TransportLog() {
                        TransportTypeId = TransportTypeId,
                        Method = null,
                        Outcome = _outcome,
                        Url = null,
                        Request = null,
                        StatusCode = statusCode,
                        Response = response,
                    });
                    entities.SaveChanges();
                }
            }
        }

        public void Exception(object reqObj, Exception e) {
            bool hasId = GetRecordId(reqObj, out long id);

            using (ProtEntities entities = new ProtEntities()) {
                if (hasId) {
                    entities.TransportLog.Where(l => l.Id == id)
                        .Update(l => new TransportLog() {
                            Exception = e.ToString(),
                        });
                    //entities.SaveChanges();
                } else {
                    entities.TransportLog.Add(new TransportLog() {
                        TransportTypeId = TransportTypeId,
                        Method = null,
                        Outcome = _outcome,
                        Url = null,
                        Request = null,
                        Exception = e.ToString()                        
                    });
                    entities.SaveChanges();
                }
            }
        }

        private bool GetRecordId(object reqObj, out long id) {
            lock (_lock) {
                if (_data.TryGetValue(reqObj, out id)) {
                    _data.Remove(reqObj);
                    return true;
                } else {
                    id = 0;
                    return false;
                }
            }
        }
    }
}
