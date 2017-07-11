using Krypton.LibProtocol.Member.Type;

namespace Krypton.LibProtocol.Member.Operation.Meta
{
    public class EnumerableOperation : MetaOperation, ITypeContainer
    {
        public string Name { get; internal set; }
        public TypeReference Type { get; internal set; }
        
        public void AcquireTypeReference(TypeReference reference)
        {
            Type = reference;
        }
    }
}