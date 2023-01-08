using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Devino.Request
{
    public class SourceAddressesRequest : RequestBase<SourceAddressesResponse>
    {
        public SourceAddressesRequest() : base("UserSettings/SourceAddresses", HttpMethod.Get)
        {
            Params.Add( new Param("format", "json"));
        }
    }
}
