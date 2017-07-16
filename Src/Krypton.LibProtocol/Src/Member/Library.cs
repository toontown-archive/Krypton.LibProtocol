using System.Collections.Generic;
using Krypton.LibProtocol.Member.Common;
using Krypton.LibProtocol.Member.Declared;
using Krypton.LibProtocol.Member.Type;

namespace Krypton.LibProtocol.Member
{
    public class Library : ICustomizable
    {
        /// <summary>
        /// The namespace containing the Libraries' packets and types
        /// </summary>
        [Option("namespace")]
        public string Namespace { get; set; }
        
        public IList<Packet> Packets { get; }
        public IList<IDeclaredType> Types { get; }
        
        /// <summary>
        /// The alias used to reference the Library inside the KPDL
        /// </summary>
        public string Name { get; internal set; }

        public Library()
        {
            Packets = new List<Packet>();
            Types = new List<IDeclaredType>();
        }

        public void AddType(IDeclaredType type)
        {
            Types.Add(type);
        }

        public void AddPacket(Packet packet)
        {
            Packets.Add(packet);
        }
    }
}
