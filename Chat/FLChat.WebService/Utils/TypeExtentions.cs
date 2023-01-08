using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.Utils
{
    static public class TypeExtentions
    {
        static public string GetJsonPropertyName(this Type T, string name) 
        {
            JsonObjectAttribute objAttr = T.GetCustomAttribute<JsonObjectAttribute>();
            NamingStrategy ns = null;
            if (objAttr != null && objAttr.NamingStrategyType != null)
                ns = Activator.CreateInstance(objAttr.NamingStrategyType, objAttr.NamingStrategyParameters) as NamingStrategy;
            //else
            //    ns = new DefaultNamingStrategy();

            return GetJsonPropertyName(T, name, ns);
        }

        static public string GetJsonPropertyName(this Type T, string name, NamingStrategy ns) {
            MemberInfo propertyT = T.GetProperty(name);
            if (propertyT == null)
                propertyT = T.GetField(name);

            if (propertyT == null)
                return null;

            if (propertyT.GetCustomAttribute<JsonIgnoreAttribute>() != null)
                return null;

            var attribute = propertyT.GetCustomAttribute<JsonPropertyAttribute>(true);
            if (attribute != null) {
                return attribute.PropertyName ?? name;
            } else
                return ns != null ? ns.GetPropertyName(name, false) : name;
        }

        /// <summary>
        /// Get json serialize names for all properties of type <paramref name="T"/>
        /// </summary>
        /// <param name="T">type</param>
        /// <returns>list of pairs {name of type property, name of json property}</returns>
        static public IEnumerable<Tuple<string, string>> GetJsonPropertiesName(this Type T) {
            JsonObjectAttribute objAttr = T.GetCustomAttribute<JsonObjectAttribute>();
            NamingStrategy ns = null;
            if (objAttr != null && objAttr.NamingStrategyType != null)
                ns = Activator.CreateInstance(objAttr.NamingStrategyType, objAttr.NamingStrategyParameters) as NamingStrategy;

            return T.GetProperties()
                .Select(pi => pi.Name)
                .Concat(T.GetFields().Select(fi => fi.Name))
                .Select(n => Tuple.Create(n, GetJsonPropertyName(T, n)))
                .Where(t => t.Item2 != null);
        }

        /// <summary>
        /// Находит названия полей класса в json-сериализации, если нет - оставляет старое
        /// </summary>
        /// <param name="T"></param>
        /// <param name="stringArr">Строки, у которых надо найти назваия json-сериализации</param>
        /// <returns>Найденное название, оригинальное название</returns>
        static public Dictionary <string, string> GetJsonPropertyName(this Type T, IEnumerable<string> stringArr)
        {
            Dictionary<string, string> strDic = new Dictionary<string, string>();
            foreach (var name in stringArr)
            {
                string pname = T.GetJsonPropertyName(name);
                if(pname != null)
                  strDic.Add(pname, name);
                
            }            
            return strDic;
        }
    }
}
