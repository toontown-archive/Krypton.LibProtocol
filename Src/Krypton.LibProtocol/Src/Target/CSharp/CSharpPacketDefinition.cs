using Krypton.LibProtocol.Member;

namespace Krypton.LibProtocol.Target.CSharp
{
    public class CSharpPacketDefinitionContext : PacketDefinitionContext
    {
        public CSharpPacketDefinitionContext(IPacketStatement packet) : base(packet)
        {
        }
    }

    public class CSharpPacketDefinition : PacketDefinition
    {
        public CSharpPacketDefinition(DefinitionContext context) : base(context)
        {
        }

        public override void Build()
        {
            throw new System.NotImplementedException();
        }

        protected override DataDefinition CreateDataDefintion(DataStatement data)
        {
            throw new System.NotImplementedException();
        }

        protected override ConditionalDefinition CreateConditionalDefinition(ConditionalStatement conditional)
        {
            throw new System.NotImplementedException();
        }
    }
}