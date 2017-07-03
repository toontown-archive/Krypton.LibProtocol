using System.Collections.Generic;
using Krypton.LibProtocol.Parser;

namespace Krypton.LibProtocol.Member
{
    public class Packet : IStatementContainer
    {
        public string Name { get; }
        public IList<Packet> Parents { get; }
        public IList<PacketStatement> Statements { get; }

        internal Packet(string name)
        {
            Name = name;
            Parents = new List<Packet>();
            Statements = new List<PacketStatement>();
        }

        internal void AddParent(Packet parent)
        {
            if (Parents.Contains(parent))
            {
                throw new KryptonParserException($"Packet {Name} contains parent {parent.Name} more than once.");
            }
            
            Parents.Add(parent);
        }

        public void AddStatement(PacketStatement statement)
        {
            var data = statement as DataStatement;
            if (data != null)
            {
                AddStatement(data);
            }
            else
            {
                Statements.Add(statement);
            }

        }

        internal void AddStatement(DataStatement statement)
        {
            // todo: check if the name has already been defined
            Statements.Add(statement);
        }
    }
}
