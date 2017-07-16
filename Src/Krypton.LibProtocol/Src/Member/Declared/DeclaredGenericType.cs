using System.Collections.Generic;
using Krypton.LibProtocol.Member.Statement;

namespace Krypton.LibProtocol.Member.Declared
{
    public class DeclaredGenericType : IDeclaredType
    {
        public Library Parent { get; }
        public string Name { get; set; }
        public StatementBlock Statements { get; }

        public IList<string> Generics { get; }

        public DeclaredGenericType(Library parent)
        {
            Parent = parent;
            Generics = new List<string>();
            Statements = new StatementBlock();
        }
    }
}
