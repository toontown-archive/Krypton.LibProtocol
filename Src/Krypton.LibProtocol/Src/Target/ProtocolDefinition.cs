using Proto = Krypton.LibProtocol.Member;

namespace Krypton.LibProtocol.Target
{
    public abstract class ProtocolDefinitionContext : DefinitionContext
    {
        public Proto.Protocol Protocol { get; }

        protected ProtocolDefinitionContext(Proto.Protocol protocol)
        {
            Protocol = protocol;
        }

        public abstract void AddMessageDefinition(MessageDefinitionContext context);

        public abstract void AddPacketDefinition(PacketDefinitionContext context);
    }
    
    public abstract class ProtocolDefinition : Definition
    {
        protected ProtocolDefinition(DefinitionContext context) : base(context)
        {
        }
        
        /// <summary>
        /// Creates a new MessageDefinition instance
        /// </summary>
        /// <returns></returns>
        protected abstract MessageDefinition CreateMessageDefinition(Proto.Message message);

        /// <summary>
        /// Creates a new PacketDefinition instance
        /// </summary>
        /// <returns></returns>
        protected abstract PacketDefinition CreatePacketDefinition(Proto.Packet packet);

        public override void Build()
        {
            var ctx = (ProtocolDefinitionContext) Context;
            
            // create each message
            foreach (var msg in ctx.Protocol.Messages)
            {
                var mdef = CreateMessageDefinition(msg);
                mdef.Build();
                
                ctx.AddMessageDefinition((MessageDefinitionContext)mdef.Context);
            }
            
            // create each packet
            foreach (var packet in ctx.Protocol.Packets)
            {
                var pdef = CreatePacketDefinition(packet);
                pdef.Build();
                
                ctx.AddPacketDefinition((PacketDefinitionContext)pdef.Context);
            }
        }
    }
}
