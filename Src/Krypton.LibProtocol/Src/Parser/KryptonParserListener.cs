using System;
using System.Collections.Generic;
using System.Linq;
using Krypton.LibProtocol.File;
using Krypton.LibProtocol.Member;
using Krypton.LibProtocol.Member.Common;
using Krypton.LibProtocol.Member.Declared;
using Krypton.LibProtocol.Member.Declared.Type;
using Krypton.LibProtocol.Member.Statement;
using Krypton.LibProtocol.Member.Type;

namespace Krypton.LibProtocol.Parser
{
    public class KryptonParserListener : KryptonParserBaseListener
    {
        private const string _nsDelimiter = "::";
        private const string _thisKeyword = "this";
        private readonly DefinitionFile _file;
        private readonly Stack<ICustomizable> _customizables;
        private readonly Stack<IMemberContainer> _memberContainers;
        private readonly Stack<IStatementContainer> _statementContainers;
        private readonly Stack<ITypeReferenceContainer> _typeReferenceContainers;
        private readonly Stack<IMemberContainer> _contextStack;
        
        public KryptonParserListener(DefinitionFile file)
        {
            _file = file;
            _customizables = new Stack<ICustomizable>();
            _memberContainers = new Stack<IMemberContainer>();
            _statementContainers = new Stack<IStatementContainer>();
            _typeReferenceContainers = new Stack<ITypeReferenceContainer>();
            _contextStack = new Stack<IMemberContainer>();
            
            // the definition file is our root container and context
            _memberContainers.Push(_file);
            _contextStack.Push(_file);
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
        /// Library declaration entry.
        /// 
        /// LIBRARY IDENTIFIER '{' member_options? namespace_member* '}' 
        /// </summary>
        /// <param name="context"></param>
        public override void EnterLibrary_declaration(KryptonParser.Library_declarationContext context)
        {
            var parent = _memberContainers.Peek();
            var name = context.IDENTIFIER().GetText();
            
            var lib = new Library(name, parent);
            parent.AddMember(lib);
            
            _memberContainers.Push(lib);
            _contextStack.Push(lib);
            _customizables.Push(lib);
        }


        /// <summary>
        /// Library declaration departure.
        /// </summary>
        /// <param name="context"></param>
        public override void ExitLibrary_declaration(KryptonParser.Library_declarationContext context)
        {
            Console.Out.WriteLine(_contextStack.Count); //dbg
            _memberContainers.Pop();
            _contextStack.Pop();
            _customizables.Pop();
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

            var parent = _memberContainers.Peek();
            var activeContext = _contextStack.Peek();
            
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
        /// Type reference entry.
        /// 
        /// (namespace_reference '::')? type_name generic_types?
        /// </summary>
        /// <param name="context"></param>
        public override void EnterType_reference(KryptonParser.Type_referenceContext context)
        {
            // pass if we are a generic attribute reference.
            var attribute = context.generic_attribute_reference();
            if (attribute != null)
            {
                return;
            }

            var parent = _typeReferenceContainers.Peek();

            var name = context.type_name().GetText();
            var ns = context.namespace_reference()?.GetText() ?? "";
            
            // if "this" is the namespace we are referencing the local context
            // setting the namespace to nothing will make us look through the active context first
            if (ns == _thisKeyword)
            {
                ns = ""; 
            }

            var path = ns.Split(new[] {_nsDelimiter}, StringSplitOptions.RemoveEmptyEntries);
            var activeContext = _contextStack.Peek();
            
            // Resolve the member reference
            IMember member;
            if (!TryResolveMember(path, name, activeContext, out member))
            {
                throw new KryptonParserException($"Unable to resolve type {ns} {name}");
            }

            IType type;
            
            // If we are a generic type...
            var generic = context.generic_types() != null;
            if (generic)
            {
                type = new GenericType(name);
                _typeReferenceContainers.Push((ITypeReferenceContainer) type);
            }
            else
            {
                type = new ConcreteType(name);
            }

            // Create a type reference and add it to our parent.
            var reference = new FormalTypeReference(type, member.Parent);
            parent.AddTypeReference(reference);
        }

        /// <summary>
        /// Type reference departure.
        /// </summary>
        /// <param name="context"></param>
        public override void ExitType_reference(KryptonParser.Type_referenceContext context)
        {
            // Remove ourselves from the stack if we are a generic
            var generic = context.generic_types() != null;
            if (generic)
            {
                _typeReferenceContainers.Pop();
            }
        }

        /// <summary>
        /// Generic attribute reference handling.
        /// 
        /// IDENTIFIER
        /// </summary>
        /// <param name="context"></param>
        public override void EnterGeneric_attribute_reference(KryptonParser.Generic_attribute_referenceContext context)
        {
            var parent = _typeReferenceContainers.Peek();
            var name = context.IDENTIFIER().GetText();
            
            var type = new GenericAttribute(name);
            var reference = new FormalTypeReference(type, null);
            
            parent.AddTypeReference(reference);
        }

        /// <summary>
        /// Data statement entry.
        /// 
        /// type_reference IDENTIFIER ';'
        /// </summary>
        /// <param name="context"></param>
        public override void EnterData_statement(KryptonParser.Data_statementContext context)
        {
            var parent = _statementContainers.Peek();
            var name = context.IDENTIFIER().GetText();
            var statement = new TypeStatement(name, parent);
            
            // Add ourselves to our parent and push us onto the ref container stack
            parent.AddStatement(statement);
            _typeReferenceContainers.Push(statement);
        }

        /// <summary>
        /// Data statement departure.
        /// </summary>
        /// <param name="context"></param>
        public override void ExitData_statement(KryptonParser.Data_statementContext context)
        {
            _typeReferenceContainers.Pop();
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
