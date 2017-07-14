using System.Collections.Generic;

namespace Krypton.LibProtocol.Member.Type
{
    public class LocalGenericTypeReference : LocalTypeReference, ITypeContainer
    {
        public IList<TypeReference> Generics { get; }

        public LocalGenericTypeReference()
        {
            Generics = new List<TypeReference>();
        }

        public void AcquireTypeReference(TypeReference reference)
        {
            Generics.Add(reference);
        }
        
        public override string TemplateAlias => "local_generic_type_reference";
    }
}