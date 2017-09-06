using Krypton.LibProtocol.Target;

namespace Krypton.LibProtocol.Member.Expression
{
    public class MemberExpression : IExpression, ITemplateType
    {
        public string TemplateName => "member_expression";
        
        public IMember Member { get; }

        public MemberExpression(IMember member)
        {
            Member = member;
        }
    }
}