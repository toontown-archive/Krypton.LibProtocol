using System;
using System.ComponentModel;
using Proto = Krypton.LibProtocol.Member;

namespace Krypton.LibProtocol.Parser
{
    public class KryptonContextHandler
    {
        private KryptonFile _file;
        
        public KryptonContextHandler(KryptonFile file)
        {
            _file = file;
        }

        /// <summary>
        /// Handles parsing context
        /// </summary>
        /// <param name="context"></param>
        public void HandleInitCtx(KryptonParser.InitContext context)
        {
            // Handle each group
            foreach (var groupctx in context._groups)
            {
                HandleGroupCtx(groupctx);
            }
            
            // Handle each library
            foreach (var libraryctx in context._libraries)
            {
                HandleLibraryCtx(libraryctx);
            }
            
            // Handle each protocol
            foreach (var protocolgtx in context._protocols)
            {
                HandleProtocolCtx(protocolgtx);
            }
        }

        /// <summary>
        /// Handles group definition context
        /// </summary>
        /// <param name="context"></param>
        private void HandleGroupCtx(KryptonParser.Group_definitionContext context)
        {
            var group = new Proto.Group(context.name.Text);

            // Add the group
            KryptonParserException.Context = context.name.Line;
            _file.AddGroup(group);
        }

        /// <summary>
        /// Handles library definition context
        /// </summary>
        /// <param name="context"></param>
        private void HandleLibraryCtx(KryptonParser.Library_definitionContext context)
        {
            var library = new Proto.Library(context.name.Text);
            
            // Add the library
            KryptonParserException.Context = context.name.Line;
            _file.AddLibrary(library);
            
            // Handle each packet
            foreach (var packetctx in context._packets)
            {
                HandlePacketCtx(library, packetctx);
            }
        }

        /// <summary>
        /// Handles protocol definition context
        /// </summary>
        /// <param name="context"></param>
        private void HandleProtocolCtx(KryptonParser.Protocol_definitionContext context)
        {
            var protocol = new Proto.Protocol(context.ns.GetText(), context.name.Text, _file);
            
            // Add the protocol
            KryptonParserException.Context = context.name.Line;
            _file.AddProtocol(protocol);
            
            // Handle each statement defined inside the protocol.
            var statementsctx = context.proto_statements();

            var messages = statementsctx.messages;
            if (messages != null)
                HandleMessageCtx(protocol, messages);

            foreach (var packetctx in statementsctx._packets)
            {
                HandlePacketCtx(protocol, packetctx);
            }
        }

        /// <summary>
        /// Handles message definition context
        /// </summary>
        /// <param name="container"></param>
        /// <param name="context"></param>
        private void HandleMessageCtx(Proto.Protocol container, KryptonParser.Message_definitionsContext context)
        {
            var names = context._names;

            foreach (var name in names)
            {
                Console.Out.WriteLine(name.Text);
                var message = new Proto.Message(name.Text);

                // Add the message
                KryptonParserException.Context = name.Line;
                container.AddMessage(message);
            }
        }

        /// <summary>
        /// Handles packet definition context
        /// </summary>
        /// <param name="container"></param>
        /// <param name="context"></param>
        private void HandlePacketCtx(Proto.PacketContainer container, KryptonParser.Packet_definitionContext context)
        {
            var packet = new Proto.Packet(context.name.Text);
            
            // Add the packet
            KryptonParserException.Context = context.name.Line;
            container.AddPacket(packet);
            
            void HandleParentContext(KryptonParser.Packet_parentsContext parentctx)
            {
                KryptonParserException.Context = parentctx.name.Line;

                var parent = _file.ResolveLibraryPacket(parentctx.ns.GetText(), parentctx.name.Text);
                if (parent == null)
                {
                    throw new KryptonParserException($"Packet {parentctx.name.Text} does not exist.");
                }

                packet.AddParent(parent);

                var nested = parentctx.packet_parents();
                foreach (var p in nested)
                {
                    HandleParentContext(p);
                }
            }

            // Add each parent
            var rootParent = context.packet_parents();
            if (rootParent != null)
            {
                HandleParentContext(rootParent);
            }

            // Handle each statement context
            var statements = context.packet_statement();
            foreach (var statementctx in statements)
            {
                HandlePacketStatementCtx(packet, statementctx);
            }
        }

        /// <summary>
        /// Handles packet statement context
        /// </summary>
        /// <param name="container"></param>
        /// <param name="context"></param>
        private void HandlePacketStatementCtx(Proto.StatementContainer container, KryptonParser.Packet_statementContext context)
        {
            // Data statement
            if (context.datadef != null)
            {
                var datadef = context.datadef;
                    
                var type = (Proto.Primitive)Enum.Parse(typeof(Proto.Primitive), datadef.type.Text, true);
                var data = new Proto.DataStatement(type, datadef.name.Text);

                KryptonParserException.Context = datadef.name.Line;
                container.AddStatement(data);
            }
            
            // Conditional statement
            else
            {
                var condef = context.condef;

                var opRaw = condef.condition().op.Text;
                var op = Proto.ConditionalStatement.OperatorFromString(opRaw);

                if (op == Proto.Operator.Unknown)
                {
                    KryptonParserException.Context = condef.condition().op.Line;
                    throw new KryptonParserException($"Unknown operator {opRaw}.");
                }

                var condition = new Proto.Condition
                {
                    Val1 = condef.condition().val1.GetText(),
                    Val2 = condef.condition().val2.GetText(),
                    Op = op
                };
                
                var conditional = new Proto.ConditionalStatement(condition);
                container.AddStatement(conditional);
                
                // Add the child statements
                foreach (var statementctx in condef._statements)
                {
                    HandlePacketStatementCtx(conditional, statementctx);
                }
            }
        }
    }
}
