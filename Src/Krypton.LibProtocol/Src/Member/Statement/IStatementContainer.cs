using System.Collections.Generic;

namespace Krypton.LibProtocol.Member.Statement
{
    public interface IStatementContainer
    {
        IEnumerable<IStatement> Statements { get; }

        void AddStatement(IStatement statement);
    }
}
