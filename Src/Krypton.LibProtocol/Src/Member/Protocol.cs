namespace Krypton.LibProtocol.Member
{
    public class Protocol : NestedMemberContainer, IMember
    {        
        public string Name { get; }

        public Protocol(string name, IMemberContainer parent) : base(parent)
        {
            Name = name;
        }
    }
    
    public class Message : IMember
    {
        public int Id { get; }
        public string Name { get; }
        public IMemberContainer Parent { get; }

        public Message(string name, int id, IMemberContainer parent)
        {
            Name = name;
            Id = id;
            Parent = parent;
        }
    }
}
