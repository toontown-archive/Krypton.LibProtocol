namespace Krypton.LibProtocol.Member.Type
{
    public interface ITypeReference
    {
        IType Type { get; }
        
        IMemberContainer Scope { get; }
    }
}
