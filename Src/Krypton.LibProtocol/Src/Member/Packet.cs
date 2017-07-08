using System;
using System.Collections.Generic;
using Krypton.LibProtocol.Parser;

namespace Krypton.LibProtocol.Member
{
    public class Packet : StatementContainer
    {
        public string Name { get; }
        public IList<Packet> Parents { get; }

        internal Packet(string name)
        {
            Name = name;
            Parents = new List<Packet>();
        }

        internal void AddParent(Packet parent)
        {
            if (Parents.Contains(parent))
            {
                throw new KryptonParserException($"Packet {Name} contains parent {parent.Name} more than once.");
            }

            Parents.Add(parent);
        }

        public override void AddStatement(IPacketStatement statement)
        {
            var data = statement as DataStatement;
            if (data != null)
            {
                AddStatement(data);
            }
            else
            {
                AddStatement(statement as ConditionalStatement);
            }
        }

        private void AddStatement(DataStatement statement)
        {
            // todo: checks
            Statements.Add(statement);
        }

        private void AddStatement(ConditionalStatement statement)
        {
            // todo: checks
            Statements.Add(statement);
        }

        public List<IPacketStatement> InheritedStatements
        {
            get
            {
                var statements = new List<IPacketStatement>();

                foreach (var parent in Parents)
                {
                    statements.AddRange(parent.InheritedStatements);
                    statements.AddRange(parent.Statements);
                }
                return statements;
            }
        }
    }
}
