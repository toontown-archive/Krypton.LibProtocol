using Krypton.LibProtocol.Member.Common;
using Krypton.LibProtocol.Target;

namespace Krypton.LibProtocol.Member
{
    public class Protocol : NestedMemberContainer, IMember, ITemplateType, INameable, IDocumentable
    {        
        public string TemplateName => "protocol";
        
        /// <summary>
        /// The name of the protocol
        /// </summary>
        public string Name { get; }

        public Documentation Documentation { get; private set; }
        
        public Protocol(string name, IMemberContainer parent) : base(parent)
        {
            Name = name;
        }

        public void SetDocumentation(Documentation documentation)
        {
            Documentation = documentation;
        }
    }
    
    public class Message : IMember, ITemplateType, INameable, IDocumentable
    {
        public string TemplateName => "message";
        
        public int Id { get; }
        public string Name { get; }
        public IMemberContainer Parent { get; }
        public Documentation Documentation { get; private set; }

        public Message(string name, int id, IMemberContainer parent)
        {
            Name = name;
            Id = id;
            Parent = parent;
        }
        
        public void SetDocumentation(Documentation documentation)
        {
            Documentation = documentation;
        }
    }
}
