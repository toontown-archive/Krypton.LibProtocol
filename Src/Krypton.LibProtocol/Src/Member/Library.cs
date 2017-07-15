using System.Collections.Generic;
using Krypton.LibProtocol.Member.Common;
using Krypton.LibProtocol.Member.Type;

namespace Krypton.LibProtocol.Member
{
    public class Library : IPacketContainer, ICustomizable
    {
        /// <summary>
        /// The namespace containing the Libraries' packets and types
        /// </summary>
        [Option("namespace")]
        public string Namespace { get; set; }
        
        public IList<Packet> Packets { get; }
        public IList<TypeDeclaration> Types { get; }
        
        /// <summary>
        /// The alias used to reference the Library inside the KPDL
        /// </summary>
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