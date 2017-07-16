using Krypton.LibProtocol.Member.Type;

namespace Krypton.LibProtocol.Parser
{
    internal interface ITypeParent
    {
        void AcquireType(TypeReference type);
    }
}
