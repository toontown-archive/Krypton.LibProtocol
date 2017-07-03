using System;

namespace Krypton.LibProtocol.Target
{
    public abstract class ProtocolDefinitionContext : DefinitionContext
    {
        
        
    }
    
    public abstract class ProtocolDefinition : TargetDefinable
    {
        /// <summary>
        /// Creates a new MessageDefinition instance
        /// </summary>
        /// <returns></returns>
        protected abstract MessageDefinition CreateMessageDefinition();

        /// <summary>
        /// Creates a new PacketDefinition instance
        /// </summary>
        /// <returns></returns>
        protected abstract PacketDefinition CreatePacketDefinition();
    }
}
