using System.Collections.Generic;
using Krypton.LibProtocol.Member.Common;
using Krypton.LibProtocol.Member.Declared;
using Krypton.LibProtocol.Target;

namespace Krypton.LibProtocol.Member
{
    public struct ProtocolPair
    {
        public Packet Packet { get; set; }
        public Message Message { get; set; }

        public void Include(Packet packet)
        {
            Packet = packet;
        }
        
        public void Include(Message message)
        {
            Message = message;
        }
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

        public IEnumerable<ProtocolPair> Pairs {
            get
            {
                var pairs = new Dictionary<string, ProtocolPair>();

                foreach (var member in Members)
                {
                    var name = member.Name;
                    if (!pairs.TryGetValue(name, out var pair))
                    {
                        pair = new ProtocolPair();
                    }

                    if (member is Message)
                    {
                        pair.Include((Message)member);
                    }
                    else if (member is Packet)
                    {
                        pair.Include((Packet)member);
                    }

                    pairs[name] = pair;
                }

                return pairs.Values;
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
