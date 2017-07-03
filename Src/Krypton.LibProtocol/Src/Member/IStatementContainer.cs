using System.Collections.Generic;

namespace Krypton.LibProtocol.Member
{
    public interface IStatementContainer
    {
        IList<PacketStatement> Statements { get; }

        void AddStatement(PacketStatement statement);
    }
}