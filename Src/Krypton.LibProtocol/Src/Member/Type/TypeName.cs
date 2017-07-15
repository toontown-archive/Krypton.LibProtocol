using System.Collections.Generic;
using Krypton.LibProtocol.Extensions;

namespace Krypton.LibProtocol.Member.Type
{
    public class TypeName
    {
        public string Name { get; internal set; }     
        public string CamelCaseName => Name.ToCamelCase();
        
        public IList<string> Generics { get; }

        public bool HasGenerics => Generics.Count > 0;
        
        public TypeName()
        {
            Generics = new List<string>();
        }
    }
}
