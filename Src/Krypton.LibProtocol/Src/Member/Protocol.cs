using System.Collections.Generic;
using Krypton.LibProtocol.Parser;

namespace Krypton.LibProtocol.Member
{
    public class Protocol : PacketContainer
    {
        public KryptonFile File { get; }
        public string Namespace { get; }
        internal IList<Message> Messages { get; }
        
        internal Protocol(string ns, string name, KryptonFile file) : base(name)
        {
            File = file;
            Namespace = ns;
            Messages = new List<Message>();
        }

        internal void AddMessage(Message message)
        {
            // Verify this message hasnt been defined
            foreach (var m in Messages)
            {
                if (m.Name == message.Name)
                {
                    throw new KryptonParserException($"Message {m.Name} is already defined");
                }
            }
            
            Messages.Add(message);
            File.AddMessage(message);
        }
    }
}