using System.Collections.Generic;
using Krypton.LibProtocol.Member.Common;
using Krypton.LibProtocol.Member.Declared;
using Krypton.LibProtocol.Target;

namespace Krypton.LibProtocol.Member
{
    public class ProtocolPair : IMember, ITemplateType, INameable
    {
        public string TemplateName => "protocol_pair";
        
        /// <summary>
        /// The name of the pair
        /// </summary>
        public string Name { get; set; }

        public Packet Packet { get; set; }
        public Message Message { get; set; }
        
        public IMemberContainer Parent { get; set; }
    }

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

        public override void AddMember(IMember member)
        {
            // if the member is a Message or Packet we will include them
            // in a pair.
            if (member is Message || member is Packet)
            {
                if (!TryFindMember(member.Name, out var existing))
                {
                    existing = new ProtocolPair
                    {
                        Name = member.Name,
                        Parent = this
                    };
                    MemberList.Add(existing);
                }
                var pair = (ProtocolPair) existing;

                var message = member as Message;
                if (message != null)
                {
                    pair.Message = message;
                }
                else
                {
                    pair.Packet = (Packet) member;
                }
            }
            else
            {
                MemberList.Add(member);
            }
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
