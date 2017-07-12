using System.Collections.Generic;

namespace Krypton.LibProtocol.Member.Type
{
    public class GenericPrimitiveTypeReference : PrimitiveTypeReference, ITypeContainer
    {
        public IList<TypeReference> Generics { get; }

        public GenericPrimitiveTypeReference()
        {
            Generics = new List<TypeReference>();
        }

        public void AcquireTypeReference(TypeReference reference)
        {
            Generics.Add(reference);
        }
    }
}
