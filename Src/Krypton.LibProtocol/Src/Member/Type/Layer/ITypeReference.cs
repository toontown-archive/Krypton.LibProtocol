using Krypton.LibProtocol.Member.Layer;

namespace Krypton.LibProtocol.Member.Type.Layer
{
    public interface ITypeReference
    {
        IType Type { get; }
        
        IMemberContainer Scope { get; }
    }
}
