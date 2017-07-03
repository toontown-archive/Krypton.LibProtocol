using System;

namespace Krypton.LibProtocol.Target
{
    public abstract class PacketDefinitionContext : DefinitionContext
    {
    }

    public abstract class PacketDefinition : TargetDefinable
    {
        /// <summary>
        /// Creates a new DataDefinition instance
        /// </summary>
        /// <returns></returns>
        protected abstract DataDefinition CreateDataDefintion();

        /// <summary>
        /// Creates a new ConditionalDefinition instance
        /// </summary>
        /// <returns></returns>
        protected abstract ConditionalDefinition CreateConditionalDefinition();
    }
}