using Krypton.LibProtocol.Target;

namespace Krypton.LibProtocol.Member
{
    public class Protocol : NestedMemberContainer, IMember, ITemplateType, INameable
    {        
        public string TemplateName => "protocol";
        
        /// <summary>
        /// The name of the protocol
        /// </summary>
        public string Name { get; }

        public Protocol(string name, IMemberContainer parent) : base(parent)
        {
            Name = name;
        }
    }
    
    public class Message : IMember, ITemplateType, INameable
    {
        public string TemplateName => "message";
        
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
