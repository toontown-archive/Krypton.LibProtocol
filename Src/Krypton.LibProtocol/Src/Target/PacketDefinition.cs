using Proto = Krypton.LibProtocol.Member;

namespace Krypton.LibProtocol.Target
{
    public abstract class PacketDefinitionContext : DefinitionContext
    {
        public Proto.Packet Packet { get; }

        protected PacketDefinitionContext(Proto.Packet packet)
        {
            Packet = packet;
        }

        public abstract void AddDataDefinition(DataDefinitionContext context);

        public abstract void AddConditionalDefinition(ConditionalDefinitionContext context);
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

        public override void Build()
        {
            var ctx = (PacketDefinitionContext) Context;
            
            // build each statement
            var statements = ctx.Packet.InheritedStatements;
            statements.AddRange(ctx.Packet.Statements);
            
            foreach (var statement in statements)
            {
                Definition def;
                var data = statement as Proto.DataStatement;
                if (data != null)
                {
                    def = CreateDataDefintion(data);
                    def.Build();
                    
                    ctx.AddDataDefinition((DataDefinitionContext)def.Context);
                }
                else
                {
                    def = CreateConditionalDefinition((Proto.ConditionalStatement)statement);
                    def.Build();
                    
                    ctx.AddConditionalDefinition((ConditionalDefinitionContext)def.Context);
                }
            }
        }
    }
}