using Proto = Krypton.LibProtocol.Member;

namespace Krypton.LibProtocol.Target
{
    public abstract class GroupDefinitionContext : DefinitionContext
    {
        public Proto.Group Group { get; }

        protected GroupDefinitionContext(Proto.Group group)
        {
            Group = group;
        }
    }

    public abstract class GroupDefinition : Definition
    {
        protected GroupDefinition(DefinitionContext context) : base(context)
        {
        }
    }
}