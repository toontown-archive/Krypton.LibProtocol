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
    public class KPDLFile
    {
        public IList<string> Files { get; }
        public FileResolver Includes { get; }
        public IList<string> Groups { get; }
        
        public KPDLFile()
        {
            Files = new List<string>();
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
                var listener = new CSharpParserListener(this); 
                
                var walker = new ParseTreeWalker();
                walker.Walk(listener, parser.init());
                listener.WriteUnits();
            } 
 
            Files.Append(Path.GetFileName(filepath)); 
        }

        internal void AddGroup(string group)
        {
            Groups.Add(group);
        }
    }
}
