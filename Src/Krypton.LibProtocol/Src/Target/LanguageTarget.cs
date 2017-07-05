using Proto = Krypton.LibProtocol.Member;

namespace Krypton.LibProtocol.Target
{
    public abstract class TargetSettings
    {
    }

    public abstract class LanguageTargetContext
    {
        protected KryptonFile File { get; }
        
        protected LanguageTargetContext(KryptonFile file)
        {
            File = file;
        }

        public abstract void AddGroupDefinition(GroupDefinitionContext context);

        public abstract void AddProtocolDefinition(ProtocolDefinitionContext context);

        public abstract void Write(TargetSettings settings);
    }

    public abstract class LanguageTarget
    {
        /// <summary>
        /// Creates a new LanguageTargetContext instance
        /// </summary>
        /// <returns></returns>
        protected abstract LanguageTargetContext CreateLanguageTargetContext(KryptonFile file);
        
        /// <summary>
        /// Creates a new GroupDefinition instance
        /// </summary>
        /// <returns></returns>
        protected abstract GroupDefinition CreateGroupDefinition(Proto.Group group);

        /// <summary>
        /// Creates a new ProtocolDefinition instance
        /// </summary>
        /// <returns></returns>
        protected abstract ProtocolDefinition CreateProtocolDefinition(Proto.Protocol protocol);

        public LanguageTargetContext Build(KryptonFile file)
        {
            // create our initial context
            var context = CreateLanguageTargetContext(file);
            
            // create each group
            foreach (var group in file.Groups)
            {
                var gdef = CreateGroupDefinition(group);
                gdef.Build();
                
                context.AddGroupDefinition((GroupDefinitionContext)gdef.Context);
            }
            
            // create each protocol
            foreach (var protocol in file.Protocols)
            {
                var pdef = CreateProtocolDefinition(protocol);
                pdef.Build();
                
                context.AddProtocolDefinition((ProtocolDefinitionContext)pdef.Context);
            }

            return context;
        }
    }
}
