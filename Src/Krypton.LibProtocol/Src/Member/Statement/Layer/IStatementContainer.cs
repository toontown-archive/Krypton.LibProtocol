using System.Collections.Generic;

namespace Krypton.LibProtocol.Member.Statement.Layer
{
    public interface IStatementContainer
    {
        IEnumerable<IStatement> Statements { get; }

        void AddStatement(IStatement statement);
    }
}
