using System.Collections.Generic;
using System.Linq;
using Krypton.LibProtocol.Parser;

namespace Krypton.LibProtocol.Member
{
    public abstract class PacketContainer
    {
        public string Name { get; }
        internal IList<Packet> Packets { get; }
        
        protected PacketContainer(string name)
        {
            Name = name;
            Packets = new List<Packet>();
        }

        internal void AddPacket(Packet packet)
        {
            // Verify this packet hasnt been defined
            foreach (var p in Packets)
            {
                if (p.Name == packet.Name)
                {
                    throw new KryptonParserException($"Packet {p.Name} is already defined");
                }
            }
            
            Packets.Add(packet);
        }

        internal Packet ResolvePacket(string name)
        {
            return Packets.FirstOrDefault(packet => packet.Name == name);
        }
    }
}
