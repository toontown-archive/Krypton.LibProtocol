using System.Collections.Generic;
using System.IO;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Krypton.LibProtocol.Member;
using Krypton.LibProtocol.Parser;

namespace Krypton.LibProtocol
{
    public class KryptonFile
    {
        internal IList<Group> Groups { get; }
        internal IList<Library> Libraries { get; }
        internal IList<Protocol> Protocols { get; }
        internal IList<Message> Messages { get; }
        
        internal IList<string> Files { get; }

        public KryptonFile()
        {
            Groups = new List<Group>();
            Libraries = new List<Library>();
            Protocols = new List<Protocol>();
            Messages = new List<Message>();
            
            Files = new List<string>();
        }

        internal Library ResolveLibrary(string name)
        {
            return Libraries.FirstOrDefault(library => library.Name == name);
        }

        internal Packet ResolveLibraryPacket(string lib, string packet)
        {
            var library = ResolveLibrary(lib);
            return library?.ResolvePacket(packet);
        }

        /// <summary>
        /// Adds a protocol
        /// </summary>
        /// <param name="protocol"></param>
        /// <exception cref="KryptonParserException"></exception>
        internal void AddProtocol(Protocol protocol)
        {
            // Verify this protocol hasnt been defined
            foreach (var p in Protocols)
            {
                if (p.Namespace == protocol.Namespace && p.Name == protocol.Name)
                {
                    throw new KryptonParserException($"Protocol {p.Name} is already defined");
                }
            }
            
            Protocols.Add(protocol);
        }

        /// <summary>
        /// Adds a library
        /// </summary>
        /// <param name="library"></param>
        /// <exception cref="KryptonParserException"></exception>
        internal void AddLibrary(Library library)
        {
            // Verify this library hasnt been defined
            foreach (var l in Libraries)
            {
                if (l.Name == library.Name)
                {
                    throw new KryptonParserException($"Library {l.Name} is already defined");
                }
            }
            
            Libraries.Add(library);
        }

        /// <summary>
        /// Adds a group
        /// </summary>
        /// <param name="group"></param>
        /// <exception cref="KryptonParserException"></exception>
        internal void AddGroup(Group group)
        {
            // Verify this group hasnt been defined
            foreach (var g in Groups)
            {
                if (g.Name == group.Name)
                {
                    throw new KryptonParserException($"Group {g.Name} is already defined");
                }
            }

            // Assign the group an id and add it to our list
            group.Id = Groups.Count + 1;
            Groups.Add(group);
        }

        /// <summary>
        /// Reads a .krypton file
        /// </summary>
        /// <param name="filepath">Path to the krypton file</param>
        public void Read(string filepath)
        {
            using (var fs = new FileStream(filepath, FileMode.Open))
            {
                var inputStream = new AntlrInputStream(fs);
                var lexer = new KryptonTokens(inputStream);
                var tokens = new CommonTokenStream(lexer);
                var parser = new KryptonParser(tokens);
                
                var context = parser.init();
                var handler = new KryptonContextHandler(this);
                handler.HandleInitCtx(context);
            }

            Files.Append(Path.GetFileName(filepath));
        }
    }
}
