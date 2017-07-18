using System.Collections.Generic;
using Krypton.LibProtocol.Member.Layer;

namespace Krypton.LibProtocol.Member.Declared.Type
{
    public class DeclaredGenericType : DeclaredTypeBase
    {
        public IList<string> Generics { get; }

        internal DeclaredGenericType(string name, IMemberContainer parent) : base(name, parent)
        {
            Generics = new List<string>();
        }
    }
}
