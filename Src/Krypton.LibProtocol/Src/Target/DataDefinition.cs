using Proto = Krypton.LibProtocol.Member;

namespace Krypton.LibProtocol.Target
{
    public abstract class DataDefinitionContext : PacketStatementDefinitionContext
    {
        public Proto.DataStatement Data { get; }
        
        protected DataDefinitionContext(Proto.DataStatement data)
        {
            Data = data;
        }
    }

    public abstract class DataDefinition : PacketStatementDefinition
    {
        protected DataDefinition(DefinitionContext context) : base(context)
        {
        }
    }
}
