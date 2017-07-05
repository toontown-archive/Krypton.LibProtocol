using System.Collections.Generic;

namespace Krypton.LibProtocol.Member
{
    public abstract class StatementContainer
    {
        public IList<IPacketStatement> Statements { get; }

        internal StatementContainer()
        {
            Statements = new List<IPacketStatement>();
        }

        public abstract void AddStatement(IPacketStatement statement);
    }
}