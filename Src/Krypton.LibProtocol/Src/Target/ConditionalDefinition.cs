using Proto = Krypton.LibProtocol.Member;

namespace Krypton.LibProtocol.Target
{
    public abstract class ConditionalDefinitionContext : PacketStatementDefinitionContext
    {
        public Proto.ConditionalStatement Conditional { get; }

        protected ConditionalDefinitionContext(Proto.ConditionalStatement conditional)
        {
            Conditional = conditional;
        }
    }

    public abstract class ConditionalDefinition : PacketStatementDefinition
    {
        protected ConditionalDefinition(DefinitionContext context) : base(context)
        {
        }
    }
}
