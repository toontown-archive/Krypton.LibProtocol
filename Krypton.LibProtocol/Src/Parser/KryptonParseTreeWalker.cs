using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Krypton.LibProtocol.Parser
{
    /// <summary>
    /// Subclass of ParseTreeWalker that contains references to 
    /// the file its' walking as well as the active line number.
    /// </summary>
    public class KryptonParseTreeWalker : ParseTreeWalker
    {
        /// <summary>
        /// The active tree walker. Stored in a static context so parser exceptions 
        /// can reference the current file and line number.
        /// </summary>
        public static KryptonParseTreeWalker ActiveWalker { get; private set; }
        
        /// <summary>
        /// The path to the file that the walker is walking.
        /// </summary>
        public string Filepath { get; }
        
        /// <summary>
        /// The line the walker is on.
        /// </summary>
        public int Line { get; private set; }

        private CommonTokenStream _tokens;
        
        public KryptonParseTreeWalker(string filepath, CommonTokenStream tokens)
        {
            Filepath = filepath;
            ActiveWalker = this;
            _tokens = tokens;
        }

        public override void Walk(IParseTreeListener listener, IParseTree t)
        {
            string docText = null;
            
            // check for documentation
            var token = t as ITerminalNode;
            if (token != null)
            {
                var docTokens = _tokens.GetHiddenTokensToLeft(token.Symbol.TokenIndex);
                if (docTokens != null)
                {
                    docText = string.Join("\n", docTokens.Select(x => x.Text));
                }
            }

            // update the current line
            var node = t as IRuleNode;
            if (node != null)
            {
                var ctx = (ParserRuleContext) node.RuleContext;
                Line = ctx.Start.Line;
            }

            try
            {
                base.Walk(listener, t);
                
                // pass documentation if there was any
                var kryptonListener = listener as KryptonParserListener;
                if (kryptonListener != null && docText != null)
                {
                    kryptonListener.EnterDocumentation(docText);
                }
            }          
            // Rethrow any parser exceptions thrown while walking (avoids stack trace)
            catch (KryptonParserException e)
            {
                throw e;
            }
        }
    }
}
