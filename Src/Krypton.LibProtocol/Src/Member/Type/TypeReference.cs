using Krypton.LibProtocol.Member.Type.Scope;

namespace Krypton.LibProtocol.Member.Type
{
    /// <summary>
    /// A TypeReference contains information on a Type and its Scope.
    /// The Scope is relative to the context it is beclared in.
    /// </summary>
    public class TypeReference
    {
        public IType Type { get; internal set; }
        public ITypeScope Scope { get; internal set; }
    }
}
