using System.CodeDom;

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
    }
}