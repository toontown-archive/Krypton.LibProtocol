using System.Collections.Generic;
using System.IO;
using Antlr4.Runtime;
using Krypton.LibProtocol.Member;
using Krypton.LibProtocol.Parser;

namespace Krypton.LibProtocol
{
    public class KPDLFile
    {
        public FileResolver Includes { get; }
        public IList<Group> Groups { get; }
        
        public IList<Library> Libraries { get; }
        
        public KPDLFile()
        {
            Includes = new FileResolver();
            Groups = new List<Group>();
            Libraries = new List<Library>();
        }

        internal void AddLibrary(Library library)
        {
            Libraries.Add(library);
        }

        internal void AddGroup(Group group)
        {
            group.Id = Groups.Count;
            Groups.Add(group);
        }
        
        /// <summary> 
        /// Loads in .kpdl file context
        /// </summary> 
        /// <param name="filepath">Path to the kpdl file</param> 
        public void Load(string filepath) 
        { 
            filepath = Includes.Resolve(filepath); 
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
