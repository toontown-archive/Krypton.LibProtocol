using Krypton.LibProtocol.Target;

namespace Krypton.LibProtocol.Member.Expression
{
    public class NumericalExpression : IExpression, ITemplateType
    {
        public string TemplateName => "numerical_expression";
        
        public int Value { get; }

        public NumericalExpression(int value)
        {
            Value = value;
        }
    }
}
