using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ViennaConsole
{
    public static class AssemblyExtensions
    {
        public static string GetManifestResourceString(this Assembly assembly, string resourceName)
        {
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static byte[] GetManifestResourceBytes(this Assembly assembly, string name)
        {
            using (var stream = assembly.GetManifestResourceStream(name))
            {
                if (stream == null) throw new Exception("Assembly resource not found: " + name);
                using (var reader = new BinaryReader(stream))
                {
                    return reader.ReadBytes((int)stream.Length);
                }
            }
        }

        public static IEnumerable<T> ActivateAnyAssignableFrom<T>(this Assembly assembly)
        {
            //activitate new instances of those types
            var instances = new List<T>();
            foreach (var t in GetTypesAssignableFrom<T>(assembly))
            {
                if (t.IsAbstract || t.IsInterface) continue;
                instances.Add((T)Activator.CreateInstance(t));
            }
            return instances;
        }

        public static IEnumerable<Type> GetTypesAssignableFrom<T>(this Assembly assembly)
        {
            return assembly.GetTypes().Where(typeof(T).IsAssignableFrom);
        }

        public static Dictionary<Type, T> GetTypesWithAttribute<T>(this Assembly assembly) where T : class
        {
            var hash = new Dictionary<Type, T>();
            foreach (var type in assembly.GetTypes())
            {
                var attributes = type.GetCustomAttributes(typeof(T), false).ToArray();
                if (attributes.Length > 0)
                {
                    hash.Add(type, attributes[0] as T);
                }
            }
            return hash;
        }
    }
}
