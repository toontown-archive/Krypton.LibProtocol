using Krypton.LibProtocol.Target;

// Statically typed languages don't allow for integer/boolean comparisons.
// The BooleanExpression was added so target generators would be fully aware of the
// expressions they generate. This addition allows generators to decide how they want
// to handle boolean values.

namespace Krypton.LibProtocol.Member.Expression
{
    public class BooleanExpression : IExpression, ITemplateType
    {
        public string TemplateName => "boolean_expression";
        
        public bool Value { get; }

        public BooleanExpression(bool value)
        {
            Value = value;
        }
    }
}
