using Krypton.LibProtocol.Target;

namespace Krypton.LibProtocol.Member.Expression
{
    /// <summary>
    /// An <c>IExpression</c> that references an <c>INameable</c>
    /// </summary>
    public class NameableExpression : IExpression, ITemplateType
    {
        public string TemplateName => "nameable_expression";
        
        public INameable Nameable { get; }

        public NameableExpression(INameable nameable)
        {
            Nameable = nameable;
        }
    }
}