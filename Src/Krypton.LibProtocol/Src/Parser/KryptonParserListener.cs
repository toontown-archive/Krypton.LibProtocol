using System.Collections.Generic;
using Krypton.LibProtocol.File;
using Krypton.LibProtocol.Member;
using Krypton.LibProtocol.Member.Common;
using Krypton.LibProtocol.Member.Declared.Type;
using Krypton.LibProtocol.Member.Layer;
using Krypton.LibProtocol.Member.Statement.Layer;

namespace Krypton.LibProtocol.Parser
{
    public class KryptonParserListener : KryptonParserBaseListener
    {
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
        }

        /// <summary>
        /// Builds the root container.
        /// The root container contains every definition and declaration inside the kpdl.
        /// </summary>
        /// <param name="name"></param>
        internal void BuildRootContainer(string name)
        {
            var ns = new Namespace
            {
                Name = name
            };
            
            _customizables.Push(ns);
            _memberContainers.Push(ns);
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
            var ns = new Namespace
            {
                Name = context.IDENTIFIER().GetText(), 
                Parent = parent
            };
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
            var group = new Group
            {
                Name = context.IDENTIFIER().GetText(),
                Parent = parent
            };
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

            DeclaredTypeBase declared;
            var name = context.IDENTIFIER().GetText();
            
            // Declare a generic if we have attributes
            var generics = context.generic_type_attributes()?.IDENTIFIER();
            if (generics != null)
            {
                var generic = new DeclaredGenericType(name, parent);
                foreach (var g in generics)
                {
                    generic.Generics.Add(g.GetText());
                }
                declared = generic;
            }
            else
            {
                declared = new DeclaredType(name, parent);
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
            
            var message = new Message(name, parent);
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
    }
}
