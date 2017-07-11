using System.Collections.Generic;

namespace Krypton.LibProtocol.Member.Type
{
    public class TypeName
    {
        public string Name { get; internal set; }
        
        public IList<string> Generics { get; }

        public TypeName()
        {
            Generics = new List<string>();
        }
    }
}
