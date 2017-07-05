namespace Krypton.LibProtocol.Target
{
    public abstract class DefinitionContext
    {
    }
    
    public abstract class Definition
    {
        public DefinitionContext Context { get; }

        protected Definition(DefinitionContext context)
        {
            Context = context;
        }

        public abstract void Build();
    }
}
