using System.Collections.Generic;
using System.Collections.ObjectModel;
using Krypton.LibProtocol.Member.Layer;

namespace Krypton.LibProtocol.Member
{
    public class Protocol : IMember, IMemberContainer
    {        
        public string Name { get; }
        public IMemberContainer Parent { get; }
        public IEnumerable<IMember> Members { get; }
        private readonly IList<IMember> _members = new List<IMember>();

        public Protocol(string name, IMemberContainer parent)
        {
            Name = name;
            Parent = parent;
            Members = new ReadOnlyCollection<IMember>(_members);
        }
        
        public void AddMember(IMember member)
        {
            _members.Add(member);
        }
    }
    
    public class Message : IMember
    {
        public string Name { get; }
        public IMemberContainer Parent { get; }

        public Message(string name, IMemberContainer parent)
        {
            Name = name;
            Parent = parent;
        }
    }
}
