using System.CodeDom;
using Krypton.LibProtocol.Member.Type;

namespace Krypton.LibProtocol.Target.CSharp
{
    public static class MemberExtensions
    {
        private static CodeTypeReference AsTypeReference(this PrimitiveTypeReference reference)
        {
            return new CodeTypeReference($"Krypton.LibProtocol.Type.{reference.Name}");
        }

        public static CodeTypeReference AsTypeReference(this TypeReference reference)
        {
            if (reference is PrimitiveTypeReference)
            {
                return AsTypeReference((PrimitiveTypeReference) reference);
            }
            
            return new CodeTypeReference("void");
        }
    }
}