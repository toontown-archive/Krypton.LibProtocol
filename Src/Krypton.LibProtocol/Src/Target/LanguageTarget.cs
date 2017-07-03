using System.Collections.Generic;
using Proto = Krypton.LibProtocol.Member;

namespace Krypton.LibProtocol.Target
{
    public struct TargetSettings
    {
        public string OutputDirectory { get; set; }
    }

    public abstract class LanguageTargetContext : DefinitionContext
    {
    }

    public abstract class LanguageTarget : TargetDefinable
    {
        /// <summary>
        /// Creates a new GroupDefinition instance
        /// </summary>
        /// <returns></returns>
        protected abstract GroupDefinition CreateGroupDefinition();

        /// <summary>
        /// Creates a new ProtocolDefinition instance
        /// </summary>
        /// <returns></returns>
        protected abstract ProtocolDefinition CreateProtocolDefinition();

        /// <summary>
        /// Creates a new PacketDefinition instance
        /// </summary>
        /// <returns></returns>
        protected abstract PacketDefinition CreatePacketDefinition();
    }
}
