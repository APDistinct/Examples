using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForce.Requests
{
    public abstract class GetRequestBase<TResponse> : RequestBase<object, TResponse> where TResponse : class
    {
        protected GetRequestBase(string methodName) : base(methodName, System.Net.Http.HttpMethod.Get) {
        }

        public override object RequestBody => null;
    }
}
