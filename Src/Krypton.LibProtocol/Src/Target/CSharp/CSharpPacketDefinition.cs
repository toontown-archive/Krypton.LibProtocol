using System.CodeDom;
using System.Data;
using Krypton.LibProtocol.Member;

namespace Krypton.LibProtocol.Target.CSharp
{
    public class CSharpPacketDefinitionContext : PacketDefinitionContext
    {
        public CodeTypeDeclaration Struct { get; internal set; }
        
        public CodeMemberMethod WriteMethod { get; internal set; }
        public CodeMemberMethod ReadMethod { get; internal set; }
        
        public CSharpPacketDefinitionContext(Packet packet) : base(packet)
        {
        }

        public override void AddDataDefinition(DataDefinitionContext context)
        {
            var ctx = (CSharpDataDefinitionContext) context;
            Struct.Members.Add(ctx.MemberField);
            WriteMethod.Statements.Add(ctx.WriteStatement);
            ReadMethod.Statements.Add(ctx.ReadStatement);
        }

        public override void AddConditionalDefinition(ConditionalDefinitionContext context)
        {
            var ctx = (CSharpConditionalDefinitionContext) context;

            foreach (var field in ctx.Fields)
            {
                Struct.Members.Add(field);
            }
            WriteMethod.Statements.Add(ctx.WriteStatement);
            ReadMethod.Statements.Add(ctx.ReadStatement);
        }
    }

    public class CSharpPacketDefinition : PacketDefinition
    {
        public CSharpPacketDefinition(DefinitionContext context) : base(context)
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
            var ctx = (CSharpPacketDefinitionContext) Context;

            // create the packet struct
            ctx.Struct = TargetUtil.CreateStruct(ctx.Packet.Name);

            ctx.ReadMethod = new CodeMemberMethod
            {
                Name = "Read",
                Parameters =
                {
                    new CodeParameterDeclarationExpression("Krypton.LibProtocol.BufferReader", "br")
                },
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                ReturnType = new CodeTypeReference(ctx.Packet.Name)
            };

            var declareStatement = new CodeVariableDeclarationStatement(
                ctx.Packet.Name, "__val",
                new CodeObjectCreateExpression(ctx.Packet.Name));
            ctx.ReadMethod.Statements.Add(declareStatement);
            
            ctx.WriteMethod = new CodeMemberMethod
            {
                Name = "Write",
                Parameters =
                {
                    new CodeParameterDeclarationExpression("Krypton.LibProtocol.BufferWriter", "bw")
                },
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };

            ctx.Struct.Members.Add(ctx.ReadMethod);
            ctx.Struct.Members.Add(ctx.WriteMethod);
            
            base.Build();
            
            var returnStatement = new CodeMethodReturnStatement(
                new CodeVariableReferenceExpression("__val")
                );
            ctx.ReadMethod.Statements.Add(returnStatement);
        }
    }
}