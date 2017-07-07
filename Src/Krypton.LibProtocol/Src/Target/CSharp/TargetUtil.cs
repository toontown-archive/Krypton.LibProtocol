using System;
using System.CodeDom;
using System.Linq;

namespace Krypton.LibProtocol.Target.CSharp
{
    public class TargetUtil
    {
        /// <summary>
        /// Creates a new namespace 
        /// </summary>
        /// <param name="name"></param>
        /// <returns>the namespace</returns>
        public static CodeNamespace CreateNamespace(string name)
        {
            return new CodeNamespace(name);
        }

        /// <summary>
        /// Creates a new class in the provided namespace
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ns"></param>
        /// <returns></returns>
        public static CodeTypeDeclaration CreateClass(string name, CodeNamespace ns)
        {
            var cls = new CodeTypeDeclaration(name)
            {
                IsClass = true
            };

            ns.Types.Add(cls);
            return cls;
        }
        
        public static CodeTypeDeclaration CreateClass(string name)
        {
            var cls = new CodeTypeDeclaration(name)
            {
                IsClass = true
            };
            return cls;
        }

        public static CodeTypeDeclaration CreateStruct(string name)
        {
            var cls = new CodeTypeDeclaration(name)
            {
                IsStruct = true
            };
            return cls;
        }
    }

    public static class StringExtensions
    {
        public static string ToCamelCase(this string val)
        {
            return val.Split(new [] {"_"}, StringSplitOptions.RemoveEmptyEntries).
                Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1)).
                Aggregate(string.Empty, (s1, s2) => s1 + s2);
        }
    }
}
