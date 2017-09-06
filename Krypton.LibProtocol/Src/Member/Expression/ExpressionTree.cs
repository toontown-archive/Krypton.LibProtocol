using System.Collections.Generic;
using System.Collections.ObjectModel;
using Krypton.LibProtocol.Target;

namespace Krypton.LibProtocol.Member.Expression
{
    /// <summary>
    /// An <c>ExpressionTree</c> is the barebone implementation of an <c>IExpressionContainer</c> and an <c>IExpression</c>.
    /// </summary>
    public class ExpressionTree : IExpression, IExpressionContainer, ITemplateType
    {
        public string TemplateName => "expression_tree";
        
        public IList<IExpression> Expressions { get; }

        private IList<IExpression> _expressions = new List<IExpression>();

        public ExpressionTree()
        {
            Expressions = new ReadOnlyCollection<IExpression>(_expressions);
        }

        public void AddExpresion(IExpression expression)
        {
            _expressions.Add(expression);
        }
    }
}
