using System.Collections.Generic;
using Krypton.LibProtocol.Member.Type;

namespace Krypton.LibProtocol.Member
{
    public class Library : IPacketContainer
    {
        public IList<Packet> Packets { get; }
        public IList<TypeDeclaration> Types { get; }
        
        public string Name { get; internal set; }

        public Library()
        {
            Packets = new List<Packet>();
            Types = new List<TypeDeclaration>();
        }

        public void AddType(TypeDeclaration type)
        {
            Types.Add(type);
        }

        public void AddPacket(Packet packet)
        {
            Packets.Add(packet);
        }
    }
}