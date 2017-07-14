using System.Collections.Generic;

namespace Krypton.LibProtocol.Member.Type
{
    public class DeclaredGenericTypeReference : DeclaredTypeReference, ITypeContainer
    {
        public IList<TypeReference> Generics { get; }

        public DeclaredGenericTypeReference()
        {
            Generics = new List<TypeReference>();
        }

        public void AcquireTypeReference(TypeReference reference)
        {
            Generics.Add(reference);
        }
        
        
        public override string TemplateAlias => "declared_generic_type_reference";
    }
}
