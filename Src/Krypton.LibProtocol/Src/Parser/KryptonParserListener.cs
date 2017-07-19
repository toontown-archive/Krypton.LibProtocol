using System;
using System.Collections.Generic;
using System.Linq;
using Krypton.LibProtocol.File;
using Krypton.LibProtocol.Member;
using Krypton.LibProtocol.Member.Common;
using Krypton.LibProtocol.Member.Declared;
using Krypton.LibProtocol.Member.Declared.Type;
using Krypton.LibProtocol.Member.Statement;

namespace Krypton.LibProtocol.Parser
{
    public class KryptonParserListener : KryptonParserBaseListener
    {
        private const string _nsDelimiter = "::";
        private readonly DefinitionFile _file;
        private readonly Stack<ICustomizable> _customizables;
        private readonly Stack<IMemberContainer> _memberContainers;
        private readonly Stack<IStatementContainer> _statementContainers;
        
        public KryptonParserListener(DefinitionFile file)
        {
            _file = file;
            _customizables = new Stack<ICustomizable>();
            _memberContainers = new Stack<IMemberContainer>();
            _statementContainers = new Stack<IStatementContainer>();
            
            // the definition file is our root member container
            _memberContainers.Push(_file);
        }

        /// <summary>
        /// Import statement handling. 
        /// The active file is paused till the import is read.
        /// 
        /// IMPORT (directory)? IDENTIFIER '.' KPDL ';'
        /// </summary>
        /// <param name="context"></param>
        public override void EnterImport_statement(KryptonParser.Import_statementContext context)
        {
            var dir = context.directory()?.GetText() ?? "";
            var filename = context.IDENTIFIER().GetText();
            var filepath = $"{dir}{filename}.kpdl";
            
            _file.Load(filepath);
        }

        /// <summary>
        /// Namespace declaration entry.
        /// 
        /// NAMESPACE IDENTIFIER '{' member_options? namespace_member* '}' 
        /// </summary>
        /// <param name="context"></param>
        public override void EnterNamespace_definition(KryptonParser.Namespace_definitionContext context)
        {
            var parent = _memberContainers.Peek();
            var name = context.IDENTIFIER().GetText();
            
            var ns = new Namespace(name, parent);
            parent.AddMember(ns);
            
            _memberContainers.Push(ns);
            _customizables.Push(ns);
        }

        /// <summary>
        /// Namespace declaration departure.
        /// </summary>
        /// <param name="context"></param>
        public override void ExitNamespace_definition(KryptonParser.Namespace_definitionContext context)
        {
            _memberContainers.Peek();
            _customizables.Peek();
        }

        /// <summary>
        /// Group definition handling.
        /// 
        /// GROUP IDENTIFIER ';'
        /// </summary>
        /// <param name="context"></param>
        public override void EnterGroup_definition(KryptonParser.Group_definitionContext context)
        {
            var parent = _memberContainers.Peek();
            var name = context.IDENTIFIER().GetText();

            var group = _file.GroupFactory.Create(name, parent);
            parent.AddMember(group);
            
            base.EnterGroup_definition(context);
        }

        /// <summary>
        /// Type declaration entry.
        /// 
        /// DECLARE IDENTIFIER generic_type_attributes? '{' operation_statement+ '}'
        /// </summary>
        /// <param name="context"></param>
        public override void EnterType_declaration(KryptonParser.Type_declarationContext context)
        {
            var parent = _memberContainers.Peek();

            TypeDeclarationBase declared;
            var name = context.IDENTIFIER().GetText();
            
            // Declare a generic if we have attributes
            var generics = context.generic_type_attributes()?.IDENTIFIER();
            if (generics != null)
            {
                var generic = new GenericTypeDeclaration(name, parent);
                foreach (var g in generics)
                {
                    generic.AddGeneric(g.GetText());
                }
                declared = generic;
            }
            else
            {
                declared = new TypeDeclaration(name, parent);
            }

            parent.AddMember(declared);
            _statementContainers.Push(declared);
        }

        /// <summary>
        /// Type declaration departure.
        /// </summary>
        /// <param name="context"></param>
        public override void ExitType_declaration(KryptonParser.Type_declarationContext context)
        {
            _statementContainers.Pop();
        }

        /// <summary>
        /// Packet definition entry.
        /// 
        /// PACKET IDENTIFIER (':' packet_parent)? '{' operation_statement+ '}'
        /// </summary>
        /// <param name="context"></param>
        public override void EnterPacket_definition(KryptonParser.Packet_definitionContext context)
        {
            var parent = _memberContainers.Peek();
            var name = context.IDENTIFIER().GetText();

            var packet = new Packet(name, parent);
            parent.AddMember(packet);
            
            _memberContainers.Push(packet);
            _statementContainers.Push(packet);
            _customizables.Push(packet);
        }

        /// <summary>
        /// Packet definition departure.
        /// </summary>
        /// <param name="context"></param>
        public override void ExitPacket_definition(KryptonParser.Packet_definitionContext context)
        {
            _memberContainers.Pop();
            _statementContainers.Pop();
            _customizables.Pop();
        }

        /// <summary>
        /// Packet parent handling.
        /// 
        /// (namespace_reference '::')? IDENTIFIER (',' packet_parent)?
        /// </summary>
        /// <param name="context"></param>
        public override void EnterPacket_parent(KryptonParser.Packet_parentContext context)
        {
            var ns = context.namespace_reference()?.GetText() ?? "";
            var path = ns.Split(new[] {_nsDelimiter}, StringSplitOptions.RemoveEmptyEntries);
            var name = context.IDENTIFIER().GetText();

            // active context is our parent's parent
            // we pop and push to access the container the packet was defined in
            var parent = _memberContainers.Pop();
            var activeContext = _memberContainers.Peek();
            _memberContainers.Push(parent);
            
            // Resolve the member reference
            IMember member;
            if (!TryResolveMember(path, name, activeContext, out member))
            {
                throw new KryptonParserException($"No such packet reference {ns} {name}");
            }
            
            // Check if the member is a packet and wasn't defined inside our protocol
            if (!(member is Packet) || parent.Members.Contains(member))
            {
                throw new KryptonParserException($"Unable to inherit type {ns} {name}");
            }

            // Add the member
            parent.AddMember(member);
        }

        /// <summary>
        /// Protocol declartion entry.
        /// 
        /// PROTOCOL IDENTIFIER '{' message_definitions? packet_definition* '}' 
        /// </summary>
        /// <param name="context"></param>
        public override void EnterProtocol_definition(KryptonParser.Protocol_definitionContext context)
        {
            var parent = _memberContainers.Peek();
            var name = context.IDENTIFIER().GetText();
            
            var protocol = new Protocol(name, parent);
            parent.AddMember(protocol);
            
            _memberContainers.Push(protocol);
        }

        /// <summary>
        /// Protocol declaration departure.
        /// </summary>
        /// <param name="context"></param>
        public override void ExitProtocol_definition(KryptonParser.Protocol_definitionContext context)
        {
            _memberContainers.Pop();
        }

        /// <summary>
        /// Protocol Message definition handling.
        /// 
        /// IDENTIFIER (',' message_definition)?
        /// </summary>
        /// <param name="context"></param>
        public override void EnterMessage_definition(KryptonParser.Message_definitionContext context)
        {
            var parent = _memberContainers.Peek();
            var name = context.IDENTIFIER().GetText();

            var message = _file.MessageFactory.Create(name, parent);
            parent.AddMember(message);
        }

        /// <summary>
        /// Member options
        /// </summary>
        /// <param name="context"></param>
        public override void EnterMember_option(KryptonParser.Member_optionContext context)
        {
            // todo: support more than string values
            var customizable = _customizables.Peek();

            var key = context.OPTION_KEY().GetText();
            var value = context.option_value().STRING_VAL().GetText();
            value = value.Substring(1, value.Length - 2);
            
            OptionUtil.ApplyOption(customizable, key, value);
        }

        private bool TryResolveMember(IList<string> path, string name, IMemberContainer activeContext, out IMember member)
        {
            // Try to resolve the member from the active context.
            if (activeContext.TryFindMember(path, name, out member))
            {
                return true;
            }
            
            // Try to resolve the member from the file's context.
            return _file.TryFindMember(path, name, out member);
        }
    }
}
