using System.Collections.Generic;
using System.IO;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Krypton.LibProtocol.Parser;
using Krypton.LibProtocol.Target;
using Krypton.LibProtocol.Target.CSharp;

namespace Krypton.LibProtocol
{
    public struct ParserContext
    {
        public KryptonParser.InitContext Context { get; set; }
    }

    public class KPDLFile
    {
        public IList<ParserContext> Contexts { get; }
        public FileResolver Includes { get; }
        public IList<string> Groups { get; }
        
        public KPDLFile()
        {
            Contexts = new List<ParserContext>();
            Includes = new FileResolver();
            Groups = new List<string>();
        }

        /// <summary> 
        /// Reads a .kpdl file 
        /// </summary> 
        /// <param name="filepath">Path to the kpdl file</param> 
        public void Read(string filepath) 
        { 
            filepath = Includes.Resolve(filepath); 
            using (var fs = new FileStream(filepath, FileMode.Open)) 
            { 
                var inputStream = new AntlrInputStream(fs); 
                var lexer = new KryptonTokens(inputStream); 
                var tokens = new CommonTokenStream(lexer); 
                var parser = new KryptonParser(tokens);
                
                var context = new ParserContext
                {
                    Context = parser.init()
                };
                Contexts.Append(context);
            }
        }

        internal void AddGroup(string group)
        {
            Groups.Add(group);
        }
    }
}
