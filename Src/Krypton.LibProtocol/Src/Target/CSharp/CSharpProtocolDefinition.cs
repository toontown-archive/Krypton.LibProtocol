using System;
using System.CodeDom;
using Proto = Krypton.LibProtocol.Member;

namespace Krypton.LibProtocol.Target.CSharp
{
    public class CSharpProtocolContext : ProtocolDefinitionContext
    {
        public string Outfile { get; internal set; }
        public CodeNamespace Namespace { get; internal set; }

        public CodeTypeDeclaration ProtocolDec { get; internal set; }
        public CodeTypeDeclaration MessagesDec { get; internal set; }
        public CodeTypeDeclaration PacketsDec { get; internal set; }

        public CSharpProtocolContext(Proto.Protocol protocol) : base(protocol)
        {
        }
        
        public override void AddMessageDefinition(MessageDefinitionContext context)
        {
            var ctx = (CSharpMessageDefinitionContext) context;
            MessagesDec.Members.Add(ctx.MemberField);
        }

        public override void AddPacketDefinition(PacketDefinitionContext context)
        {
            var ctx = (CSharpPacketDefinitionContext) context;
            PacketsDec.Members.Add(ctx.Struct);
        }
    }

    public class CSharpProtocolDefinition : ProtocolDefinition
    {
        public CSharpProtocolDefinition(DefinitionContext context) : base(context)
        {
        }

        protected override MessageDefinition CreateMessageDefinition(Proto.Message message)
        {
            var ctx = new CSharpMessageDefinitionContext(message);
            return new CSharpMessageDefinition(ctx);
        }

        protected override PacketDefinition CreatePacketDefinition(Proto.Packet packet)
        {
            var ctx = new CSharpPacketDefinitionContext(packet);
            return new CSharpPacketDefinition(ctx);
        }

        public override void Build()
        {
            var ctx = (CSharpProtocolContext) Context;
            
            ctx.Namespace = TargetUtil.CreateNamespace(ctx.Protocol.Namespace);
            ctx.ProtocolDec = TargetUtil.CreateClass(ctx.Protocol.Name, ctx.Namespace);
            
            ctx.MessagesDec = TargetUtil.CreateClass("Message");
            ctx.ProtocolDec.Members.Add(ctx.MessagesDec);
            
            ctx.PacketsDec = TargetUtil.CreateClass("Packet");
            ctx.ProtocolDec.Members.Add(ctx.PacketsDec);

            var fullname = $"{ctx.Protocol.Namespace}_{ctx.Protocol.Name}";
            ctx.Outfile = fullname.Replace('.', '_') + ".cs";
            
            base.Build();
        }
    }
}