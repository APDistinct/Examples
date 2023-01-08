using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SalesForce.Types;

namespace SalesForce.Requests
{
    public class QueryRequest<TRecord> : GetRequestBase<QueryResponse<TRecord>> where TRecord : class
    {
        public QueryRequest(string query) : base(@"/services/data/v47.0/query") {
            QueryParams = new Dictionary<string, string>() {
                { "q", query }
            };
        }

        public override Dictionary<string, string> QueryParams { get; }
    }
}
