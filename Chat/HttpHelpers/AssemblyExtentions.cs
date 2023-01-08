using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.Reflection
{
    public static class AssemblyExtentions
    {
        public static string LoadResource(this Assembly assembly, string resourceName) {
            return assembly.LoadResourceByFullName(String.Concat(assembly.GetName().Name, resourceName));
        }

        public static string LoadResourceByTail(this Assembly assembly, string resourceName) {
            string name = assembly.GetManifestResourceNames().Where(s => s.EndsWith(resourceName)).FirstOrDefault();
            if (name == null)
                throw new MissingMemberException(String.Concat("Resource by name ends with [", resourceName, "] has not found"));
            return assembly.LoadResourceByFullName(name);
        }

        public static string LoadResourceByFullName(this Assembly assembly, string resourceName) {
            using (Stream stream = assembly.GetManifestResourceStream(resourceName)) {
                using (StreamReader reader = new StreamReader(stream)) {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
