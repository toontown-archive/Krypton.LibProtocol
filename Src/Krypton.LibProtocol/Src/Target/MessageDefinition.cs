using Proto = Krypton.LibProtocol.Member;

namespace Krypton.LibProtocol.Target
{
    public abstract class MessageDefinitionContext : DefinitionContext
    {
        public Proto.Message Message { get; }
        
        protected MessageDefinitionContext(Proto.Message message)
        {
            Message = message;
        }
    }

    public abstract class MessageDefinition : Definition
    {
        protected MessageDefinition(DefinitionContext context) : base(context)
        {
        }
    }
}