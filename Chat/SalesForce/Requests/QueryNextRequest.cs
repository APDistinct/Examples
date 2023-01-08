using SalesForce.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForce.Requests
{
    public class QueryNextRequest<TRecord> : GetRequestBase<QueryResponse<TRecord>> where TRecord : class
    {
        public QueryNextRequest(string url) : base($@"/services/data/v47.0/query/{url}") {
        }

        public override Dictionary<string, string> QueryParams => null;
    }
}
