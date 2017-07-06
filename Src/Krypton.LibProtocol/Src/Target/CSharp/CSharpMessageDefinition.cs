using System.CodeDom;
using Krypton.LibProtocol.Member;

namespace Krypton.LibProtocol.Target.CSharp
{
    public class CSharpMessageDefinitionContext : MessageDefinitionContext
    {
        public CodeMemberField MemberField { get; internal set; }
        
        public CSharpMessageDefinitionContext(Message message) : base(message)
        {
        }
    }

    public class CSharpMessageDefinition : MessageDefinition
    {
        public CSharpMessageDefinition(DefinitionContext context) : base(context)
        {
        }

        public override void Build()
        {
            var ctx = (CSharpMessageDefinitionContext) Context;
            ctx.MemberField = new CodeMemberField
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Const,
                Name = ctx.Message.Name,
                Type = new CodeTypeReference(typeof(ushort)),
                InitExpression = new CodePrimitiveExpression(ctx.Message.Id)
            };
        }
    }
}
