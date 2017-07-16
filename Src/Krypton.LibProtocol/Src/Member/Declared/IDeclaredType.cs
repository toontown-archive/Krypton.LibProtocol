using Krypton.LibProtocol.Member.Statement;

namespace Krypton.LibProtocol.Member.Declared
{
    public interface IDeclaredType
    {
        Library Parent { get; }
        string Name { get; set; }
        
        StatementBlock Statements { get; }
    }
}