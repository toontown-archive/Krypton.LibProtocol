using System.CodeDom;
using Proto = Krypton.LibProtocol.Member;

namespace Krypton.LibProtocol.Target.CSharp
{
    public class CSharpGroupContext : GroupDefinitionContext
    {
        public CodeMemberField MemberField { get; internal set; }
        
        public CSharpGroupContext(Proto.Group group) : base(group)
        {
        }
    }

    public class CSharpGroupDefinition : GroupDefinition
    {
        public CSharpGroupDefinition(DefinitionContext context) : base(context)
        {
        }
        
        public override void Build()
        {
            var ctx = (CSharpGroupContext) Context;
            ctx.MemberField = new CodeMemberField
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Const,
                Name = ctx.Group.Name,
                Type = new CodeTypeReference(typeof(ushort)),
                InitExpression = new CodePrimitiveExpression(ctx.Group.Id)
            };
        }
    }
}
