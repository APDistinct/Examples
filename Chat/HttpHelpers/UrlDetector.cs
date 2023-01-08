using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web
{
    public class UrlDetector
    {
        private readonly Uri m_baseAddress;
        private UriTemplateMatch matches;

        public UrlDetector(string baseAddress) {
            m_baseAddress = new Uri(baseAddress);
        }

        public UriTemplateMatch Match(string strTemplate, Uri uri) {
            UriTemplate template = new UriTemplate(strTemplate);
            return template.Match(m_baseAddress, uri);
        }

        public bool IsUrl(string strTemplate, Uri uri) {
            return Match(strTemplate, uri) != null;
        }

        public bool IsUrl(string strTemplate, Uri uri, out NameValueCollection vars) {
            matches = Match(strTemplate, uri);
            if (matches != null) {
                vars = matches.BoundVariables;
                return true;
            }
            vars = null;
            return false;
        }

        public bool IsUrl(string strTemplate, Uri uri, out string key) {
            matches = Match(strTemplate, uri);
            if (matches != null) {
                key = matches.BoundVariables[0];
                return true;
            }
            key = null;
            return false;
        }        
    }
}
