using System.Collections.Generic;
using Krypton.LibProtocol.Parser;

namespace Krypton.LibProtocol.Member.Type
{
    public class GenericType : IType, ITypeParent
    {
        public string Name { get; set; }
        
        public IList<TypeReference> Types { get; }

        internal GenericType()
        {
            Types = new List<TypeReference>();
        }

        public void AcquireType(TypeReference type)
        {
            Types.Add(type);
        }
    }
}
