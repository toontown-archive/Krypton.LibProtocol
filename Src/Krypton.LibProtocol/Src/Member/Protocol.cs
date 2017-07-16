using System.Collections.Generic;
using Krypton.LibProtocol.Member.Declared;

namespace Krypton.LibProtocol.Member
{
    public class Protocol
    {        
        public string Name { get; internal set; }
        public string Namespace { get; internal set; }
        
        public IList<Packet> Packets { get; }
        public IList<string> Messages { get; }

        public Protocol()
        {
            Packets = new List<Packet>();
            Messages = new List<string>();
        }
        
        public void AddPacket(Packet packet)
        {
            Packets.Add(packet);
        }

        public void AddMessage(string message)
        {
            Messages.Add(message);
        }
    }
}
