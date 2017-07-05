using Proto = Krypton.LibProtocol.Member;

namespace Krypton.LibProtocol.Target
{
    public abstract class PacketDefinitionContext : DefinitionContext
    {
        public Proto.IPacketStatement Packet { get; }

        protected PacketDefinitionContext(Proto.IPacketStatement packet)
        {
            Packet = packet;
        }
    }

    public abstract class PacketDefinition : Definition
    {
        protected PacketDefinition(DefinitionContext context) : base(context)
        {
        }
        
        /// <summary>
        /// Creates a new DataDefinition instance
        /// </summary>
        /// <returns></returns>
        protected abstract DataDefinition CreateDataDefintion(Proto.DataStatement data);

        /// <summary>
        /// Creates a new ConditionalDefinition instance
        /// </summary>
        /// <returns></returns>
        protected abstract ConditionalDefinition CreateConditionalDefinition(Proto.ConditionalStatement conditional);
    }
}