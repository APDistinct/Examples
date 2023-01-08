using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;

namespace FLChat.WebService.Utils
{
    public interface IEnrichMethod<T> where T : class
    {
        void Enrich(ref T origin, string key);
        void Enrich(ref T origin, string name, string value);
    }

    public static class IEnrichMethodExtentions
    {
        public static void Enrich<T> (this IEnrichMethod<T> method, ref T origin, NameValueCollection parameters) where T : class {
            foreach (var key in parameters.AllKeys)
                method.Enrich(ref origin, key, parameters[key]);
        }
    }

    public class EnrichProperty<T> : IEnrichMethod<T> where T : class
    {
        private readonly static Type _t = typeof(T);
        private readonly static Dictionary<string, PropInfo> _fields;

        private class PropInfo
        {
            public PropInfo(string name) {
                Name = name;
            }

            public string Name { get; }
            public Lazy<MethodInfo> Method => new Lazy<MethodInfo>(() => {
                PropertyInfo pi = _t.GetProperty(Name);
                if (pi == null)
                    throw new Exception($"Type [{_t.Name}] has not property [{Name}]");
                MethodInfo mi = pi.GetSetMethod();
                if (mi == null)
                    throw new Exception($"Type [{_t.Name}] property [{Name}] has not set method");
                return mi;
            });
        }

        static EnrichProperty() {
            _fields = typeof(T).GetJsonPropertiesName().ToDictionary(p => p.Item2, p => new PropInfo(p.Item1));
        }

        private readonly string _propertyName;

        public EnrichProperty() : this(null) { }

        public EnrichProperty(string propertyName) {
            _propertyName = propertyName;
            //check property is correct (exist and has setter)
            if (_propertyName != null) {
                MethodInfo mi = _fields[_propertyName].Method.Value;
            }
        }

        public void Enrich(ref T origin, string key) {
            if (_propertyName == null)
                throw new NotSupportedException("Key property name was't initialize");
            Enrich(ref origin, _propertyName, key);
        }

        public void Enrich(ref T origin, string name, string value) {
            if (origin == null)
                origin = Activator.CreateInstance<T>();

            if (_fields.TryGetValue(name, out PropInfo pi))
                pi.Method.Value.Invoke(origin, new object[] { value });
            else
                throw new Exception($"Type [{typeof(T).Name}] has not property [{name}]");
        }
    }

    public class EnrichString : IEnrichMethod<string>
    {
        public void Enrich(ref string origin, string key) {
            origin = key;
        }

        public void Enrich(ref string origin, string name, string value) {
            throw new NotSupportedException();
        }
    }

    public class EnrichJObject : IEnrichMethod<JObject> 
    {
        private readonly string _propertyName;

        public EnrichJObject() : this(null) { }

        public EnrichJObject(string propertyName) {
            _propertyName = propertyName;
        }

        public void Enrich(ref JObject origin, string key) {
            if (_propertyName == null)
                throw new NotSupportedException("Key property name was't initialize");
            Enrich(ref origin, _propertyName, key);
        }

        public void Enrich(ref JObject origin, string name, string value) {
            if (origin == null)
                origin = new JObject();
            origin[name] = value;
        }
    }
}
