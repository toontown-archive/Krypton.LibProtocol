using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Krypton.LibProtocol.Parser
{
    public class KryptonParseTreeWalker : ParseTreeWalker
    {
        public override void Walk(IParseTreeListener listener, IParseTree t)
        {
            // catch any exceptions thrown during parsing
            try
            {
                base.Walk(listener, t);
            }
            catch (KryptonParserException e)
            {
                var r = (IRuleNode) t;
                var ctx = (ParserRuleContext) r.RuleContext;
                var line = ctx.Start.Line;
                Console.WriteLine(line);
                throw;
            }
        }
    }
}