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
        
        public KryptonParseTreeWalker(string filepath)
        {
            Filepath = filepath;
            ActiveWalker = this;
        }

        public override void Walk(IParseTreeListener listener, IParseTree t)
        {
            // update the current line
            if (t is IRuleNode)
            {
                var r = (IRuleNode) t;
                var ctx = (ParserRuleContext) r.RuleContext;
                Line = ctx.Start.Line;
            }

            try
            {
                base.Walk(listener, t);
            }          
            // Rethrow any parser exceptions thrown while walking (avoids stack trace)
            catch (KryptonParserException e)
            {
                throw e;
            }
        }
    }
}
