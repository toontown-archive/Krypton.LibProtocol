using System.CodeDom;
using Proto = Krypton.LibProtocol.Member;

namespace Krypton.LibProtocol.Target.CSharp
{
    public class CSharpProtocolContext : ProtocolDefinitionContext
    {
        public CodeNamespace Namespace { get; }
        private CodeTypeDeclaration _protocolDec;
        private CodeTypeDeclaration _messagesDec;
        private CodeTypeDeclaration _packetsDec;
        
        public CSharpProtocolContext(Proto.Protocol protocol) : base(protocol)
        {
            Namespace = TargetUtil.CreateNamespace(protocol.Namespace);
            _protocolDec = TargetUtil.CreateClass(protocol.Name, Namespace);
            
            _messagesDec = TargetUtil.CreateClass("Message");
            _protocolDec.Members.Add(_messagesDec);
            _packetsDec = TargetUtil.CreateClass("Packet");
            _protocolDec.Members.Add(_packetsDec);
        }

        public override void AddMessageDefinition(MessageDefinitionContext context)
        {
            var ctx = (CSharpMessageDefinitionContext) context;
            _messagesDec.Members.Add(ctx.MemberField);
        }

        public override void AddPacketDefinition(PacketDefinitionContext context)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }
    }
}