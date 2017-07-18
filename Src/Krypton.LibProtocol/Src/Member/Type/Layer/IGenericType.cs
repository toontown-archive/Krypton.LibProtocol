using System.Collections.Generic;

namespace Krypton.LibProtocol.Member.Type.Layer
{
    public interface IGenericType
    {
        IEnumerable<ITypeReference> Generics { get; }
    }
}
