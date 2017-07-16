using System.Collections.Generic;
using Krypton.LibProtocol.Member.Statement;

namespace Krypton.LibProtocol.Member.Declared
{
    public class Packet
    {
        public IList<Packet> Parents { get; }
        public string Name { get; internal set; }
        
        public StatementBlock Statements { get; }

        public Packet()
        {
            Parents = new List<Packet>();
            Statements = new StatementBlock();
        }
    }
}
