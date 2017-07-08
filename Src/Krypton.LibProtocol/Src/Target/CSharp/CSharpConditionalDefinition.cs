using System;
using Krypton.LibProtocol.Member;
using System.CodeDom;
using System.Collections.Generic;

namespace Krypton.LibProtocol.Target.CSharp
{
    public class CSharpConditionalDefinitionContext : ConditionalDefinitionContext
    {
        public List<CodeMemberField> Fields { get; internal set; }
        public CodeConditionStatement ReadStatement { get; internal set; }
        public CodeConditionStatement WriteStatement { get; internal set; }
        
        public CSharpConditionalDefinitionContext(ConditionalStatement conditional) : base(conditional)
        {
            Fields = new List<CodeMemberField>();
        }

        public override void AddDataDefinition(DataDefinitionContext context)
        {
            var ctx = (CSharpDataDefinitionContext) context;
            
            Fields.Add(ctx.MemberField);
            ReadStatement.TrueStatements.Add(ctx.ReadStatement);
            WriteStatement.TrueStatements.Add(ctx.WriteStatement);
        }

        public override void AddConditionalDefinition(ConditionalDefinitionContext context)
        {
            var ctx = (CSharpConditionalDefinitionContext) context;

            Fields.AddRange(ctx.Fields);
            ReadStatement.TrueStatements.Add(ctx.ReadStatement);
            WriteStatement.TrueStatements.Add(ctx.WriteStatement);
        }
    }

    public class CSharpConditionalDefinition : ConditionalDefinition
    {
        public CSharpConditionalDefinition(DefinitionContext context) : base(context)
        {
        }

        protected override DataDefinition CreateDataDefintion(DataStatement data)
        {
            var ctx = new CSharpDataDefinitionContext(data);
            return new CSharpDataDefinition(ctx);
        }

        protected override ConditionalDefinition CreateConditionalDefinition(ConditionalStatement conditional)
        {
            var ctx = new CSharpConditionalDefinitionContext(conditional);
            return new CSharpConditionalDefinition(ctx);
        }

        public override void Build()
        {
            var condition = new CodeBinaryOperatorExpression(
                new CodeVariableReferenceExpression("__var"), 
                GetOperatorType(), 
                new CodeVariableReferenceExpression("__var")
                );
            
            var ctx = (CSharpConditionalDefinitionContext) Context;
            ctx.ReadStatement = new CodeConditionStatement
            {
                Condition = condition
            };
            
            ctx.WriteStatement = new CodeConditionStatement
            {
                Condition = condition
            };
            
            base.Build();
        }
        
        private CodeBinaryOperatorType GetOperatorType()
        {
            var ctx = (CSharpConditionalDefinitionContext) Context;
            switch (ctx.Conditional.Condition.Op)
            {
                case Operator.Equal:
                    return CodeBinaryOperatorType.IdentityEquality;
                case Operator.Greater:
                    return CodeBinaryOperatorType.GreaterThan;
                case Operator.GreaterOrEqual:
                    return CodeBinaryOperatorType.GreaterThanOrEqual;
                case Operator.Less:
                    return CodeBinaryOperatorType.LessThan;
                case Operator.LessOrEqual:
                    return CodeBinaryOperatorType.LessThanOrEqual;
                case Operator.NotEqual:
                    return CodeBinaryOperatorType.IdentityInequality;
                default:
                    throw new Exception("todo: exception");
            }
        }
    }
}