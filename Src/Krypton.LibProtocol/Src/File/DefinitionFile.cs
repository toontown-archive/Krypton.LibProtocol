using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Antlr4.Runtime;
using Krypton.LibProtocol.File.Util;
using Krypton.LibProtocol.Member.Layer;
using Krypton.LibProtocol.Parser;

namespace Krypton.LibProtocol.File
{
    public class DefinitionFile : IMemberContainer
    {
        public IFileResolver Resolver { get; }
        public IEnumerable<IMember> Members { get; }
        private readonly IList<IMember> _members = new List<IMember>();

        public DefinitionFile(IFileResolver resolver)
        {
            Resolver = resolver;
            Members = new ReadOnlyCollection<IMember>(_members); 
        }

        public DefinitionFile()
        {
            Resolver = new ContextualFileResolver();
            Members = new ReadOnlyCollection<IMember>(_members);
        }
        
        public void AddMember(IMember member)
        {
            _members.Add(member);
        }
        
        /// <summary> 
        /// Loads in .kpdl file context
        /// </summary> 
        /// <param name="filepath">Path to the kpdl file</param> 
        public void Load(string filepath) 
        { 
            filepath = Resolver.Resolve(filepath); 
            using (var fs = new FileStream(filepath, FileMode.Open)) 
            { 
                var inputStream = new AntlrInputStream(fs);
                var lexer = new KryptonTokens(inputStream); 
                var tokens = new CommonTokenStream(lexer); 
                var parser = new KryptonParser(tokens);

                var walker = new KryptonParseTreeWalker(filepath);
                var listener = new KryptonParserListener(this);
                
                try
                {
                    walker.Walk(listener, parser.init());
                }
                // Rethrow any parser exceptions thrown while walking (avoids stack trace)
                catch (KryptonParserException e)
                {
                    throw e;
                }
            }
        }
    }
}
