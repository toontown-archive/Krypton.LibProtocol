using System.Collections.Generic;

namespace Krypton.LibProtocol.Member.Type
{
    public interface IGenericType
    {
        IEnumerable<ITypeReference> Generics { get; }
    }
}
