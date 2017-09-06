using System.Collections.Generic;

namespace Krypton.LibProtocol.Member.Expression
{
    /// <summary>
    /// A group of <c>IExpression</c>s
    /// </summary>
    public interface IExpressionContainer
    {
        /// <summary>
        /// The <c>IExpression</c>s within the container
        /// </summary>
        IList<IExpression> Expressions { get; }

        /// <summary>
        /// Adds an expression to the container
        /// </summary>
        /// <param name="expression">The expression</param>
        void AddExpresion(IExpression expression);
    }
}
