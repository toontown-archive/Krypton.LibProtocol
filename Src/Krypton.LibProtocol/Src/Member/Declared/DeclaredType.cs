using Krypton.LibProtocol.Member.Statement;

namespace Krypton.LibProtocol.Member.Declared
{
    public class DeclaredType : IDeclaredType
    {
        public Library Parent { get; }
        public string Name { get; set; }
        public StatementBlock Statements { get; }

        public DeclaredType(Library parent)
        {
            Parent = parent;
            Statements = new StatementBlock();
        }
    }
}