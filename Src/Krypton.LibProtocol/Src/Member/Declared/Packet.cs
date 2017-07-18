using System.Collections.Generic;
using Krypton.LibProtocol.Member.Common;
using Krypton.LibProtocol.Member.Layer;
using Krypton.LibProtocol.Member.Statement;

namespace Krypton.LibProtocol.Member.Declared
{
    public class Packet : IMember, IMemberContainer, ICustomizable
    {
        public string Name { get; }
        public IMemberContainer Parent { get; }
        public bool AssignParent(IMemberContainer parent)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IMember> Members { get; }
        public void AddMember(IMember member)
        {
            throw new System.NotImplementedException();
        }
    }
}
