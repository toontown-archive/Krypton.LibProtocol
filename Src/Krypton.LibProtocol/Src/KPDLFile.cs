using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Krypton.LibProtocol.Parser;

namespace Krypton.LibProtocol
{
    public class ParserContext
    {
        public KPDLFile File { get; set; }
        public string Path { get; set; }

        public void Build(KryptonParserBaseListener listener)
        {
            using (var fs = new FileStream(Path, FileMode.Open)) 
            { 
                var inputStream = new AntlrInputStream(fs);
                var lexer = new KryptonTokens(inputStream); 
                var tokens = new CommonTokenStream(lexer); 
                var parser = new KryptonParser(tokens);

                var walker = new ParseTreeWalker();
                walker.Walk(listener, parser.init());
            }
        }
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

        internal void AddGroup(string group)
        {
            Groups.Add(group);
        }
        
        /// <summary> 
        /// Loads in .kpdl file context 
        /// </summary> 
        /// <param name="filepath">Path to the kpdl file</param> 
        public void Load(string filepath) 
        { 
            var context = GenerateContext(filepath);
            Contexts.Add(context);
        }

        public ParserContext GenerateContext(string filepath)
        {
            filepath = Includes.Resolve(filepath); 
            var context = new ParserContext
            {
                File = this,
                Path = filepath
            };
            return context;
        }
    }
}
