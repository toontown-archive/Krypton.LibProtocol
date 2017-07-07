using System;
using System.CodeDom;
using Krypton.LibProtocol.Member;

namespace Krypton.LibProtocol.Target.CSharp
{
    public class CSharpDataDefinitionContext : DataDefinitionContext
    {
        public CodeMemberField MemberField { get; internal set; }

        public CodeAssignStatement ReadStatement { get; internal set; }
        
        public CodeMethodInvokeExpression WriteStatement { get; internal set; }
        
        public CSharpDataDefinitionContext(DataStatement data) : base(data)
        {
        }
    }

    public class CSharpDataDefinition : DataDefinition
    {
        public CSharpDataDefinition(DefinitionContext context) : base(context)
        {
        }

        public override void Build()
        {
            var ctx = (CSharpDataDefinitionContext) Context;
            var publicName = ctx.Data.Name.ToCamelCase();
            var type = PrimitiveAsType(ctx.Data.Type);
            
            ctx.MemberField = new CodeMemberField
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Name = publicName,
                Type = new CodeTypeReference(type)
            };
            ctx.MemberField.Name += " { get; set; }//";
            
            ctx.WriteStatement = new CodeMethodInvokeExpression(
                new CodeArgumentReferenceExpression("bw"), 
                $"Write{ctx.Data.Type}",
                new CodePropertyReferenceExpression(
                    new CodeThisReferenceExpression(), publicName)
                );

            ctx.ReadStatement = new CodeAssignStatement(
                new CodePropertyReferenceExpression(
                    new CodeVariableReferenceExpression("__val"), publicName
                    ), 
                
                new CodeMethodInvokeExpression(
                    new CodeArgumentReferenceExpression("br"), 
                    $"Read{ctx.Data.Type}"
                    )
                );
        }

        private static Type PrimitiveAsType(Primitive x)
        {
            switch (x)
            {
                case Primitive.Bool:
                    return typeof(bool);
                case Primitive.Byte:
                    return typeof(byte);
                case Primitive.Int8:
                    return typeof(sbyte);
                case Primitive.UInt8:
                    return typeof(byte);
                case Primitive.Int16:
                    return typeof(short);   
                case Primitive.UInt16:
                    return typeof(ushort);
                case Primitive.Int32:
                    return typeof(int);
                case Primitive.UInt32:
                    return typeof(uint);
                case Primitive.Int64:
                    return typeof(long);
                case Primitive.UInt64:
                    return typeof(ulong);
                case Primitive.String:
                    return typeof(string);
                case Primitive.Buffer:
                    return typeof(byte[]);
                default:
                    throw new NotImplementedException($"primitive {x} not implemented");
            }
        }
    }
}