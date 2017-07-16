using System;
using System.Collections.Generic;
using Krypton.LibProtocol.Member;
using Krypton.LibProtocol.Member.Common;
using Krypton.LibProtocol.Member.Declared;
using Krypton.LibProtocol.Member.Statement;
using Krypton.LibProtocol.Member.Type;
using Krypton.LibProtocol.Member.Type.Scope;

namespace Krypton.LibProtocol.Parser
{
    public class KryptonParserListener : KryptonParserBaseListener
    {
        protected KPDLFile File;

        protected Protocol ActiveProtocol;
        protected Library ActiveLibrary;
        protected Stack<StatementBlock> StatementBlocks;
        protected Stack<ICustomizable> Customizables;
        internal Stack<ITypeParent> TypeParents;

        public KryptonParserListener(KPDLFile file)
        {
            File = file;
            StatementBlocks = new Stack<StatementBlock>();
            TypeParents = new Stack<ITypeParent>();
            Customizables = new Stack<ICustomizable>();
        }

        public override void EnterImport_statement(KryptonParser.Import_statementContext context)
        {
            var dir = context.path?.GetText() ?? "";
            var filename = context.file.Text;
            var filepath = $"{dir}{filename}.kpdl";
            
            File.Load(filepath);
        }

        #region Type Declaration

        public override void EnterType_declaration(KryptonParser.Type_declarationContext context)
        {
            IDeclaredType declaredType;

            var generics = context.generic_type_attributes()?.IDENTIFIER();
            if (generics != null)
            {
                var genericType = new DeclaredGenericType(ActiveLibrary);
                foreach (var g in generics)
                {
                    genericType.Generics.Add(g.GetText());
                }
                declaredType = genericType;
            }
            else
            {
                declaredType = new DeclaredType(ActiveLibrary);
            }

            var name = context.IDENTIFIER().GetText();
            declaredType.Name = name;

            ActiveLibrary.AddType(declaredType);
            StatementBlocks.Push(declaredType.Statements);
        }

        public override void ExitType_declaration(KryptonParser.Type_declarationContext context)
        {
            StatementBlocks.Pop();
        }

        #endregion

        #region Type Reference

        public override void EnterBuiltin_type_reference(KryptonParser.Builtin_type_referenceContext context)
        {
            var parent = TypeParents.Peek();

            var scope = new BuiltinScope();
            IType type;
            
            var generics = context.generic_types();
            if (generics == null)
            {
                type = new Member.Type.Type();
            }
            else
            {
                type = new GenericType();
                TypeParents.Push((ITypeParent)type);
            }
            type.Name = context.BUILTIN_TYPE().GetText();

            var typeRef = new TypeReference
            {
                Type = type,
                Scope = scope
            };
            
            parent.AcquireType(typeRef);
        }

        public override void EnterDeclared_type_reference(KryptonParser.Declared_type_referenceContext context)
        {
            var parent = TypeParents.Peek();

            var scope = new DeclaredScope();
            IType type;
            
            var generics = context.generic_types();
            if (generics == null)
            {
                type = new Member.Type.Type();
            }
            else
            {
                type = new GenericType();
                TypeParents.Push((ITypeParent)type);
            }
            type.Name = context.IDENTIFIER().GetText();

            var typeRef = new TypeReference
            {
                Type = type,
                Scope = scope
            };
            
            parent.AcquireType(typeRef);
        }

        public override void EnterLocal_type_reference(KryptonParser.Local_type_referenceContext context)
        {
            var parent = TypeParents.Peek();

            var scope = new LocalScope();
            IType type;
            
            var generics = context.generic_types();
            if (generics == null)
            {
                type = new Member.Type.Type();
            }
            else
            {
                type = new GenericType();
                TypeParents.Push((ITypeParent)type);
            }
            type.Name = context.IDENTIFIER().GetText();

            var typeRef = new TypeReference
            {
                Type = type,
                Scope = scope
            };
            
            parent.AcquireType(typeRef);
        }

        public override void EnterGeneric_attribute_reference(KryptonParser.Generic_attribute_referenceContext context)
        {
            var parent = TypeParents.Peek();

            var scope = new LocalScope();
            var type = new GenericAttribute
            {
                Name = context.IDENTIFIER().GetText()
            };

            var typeRef = new TypeReference
            {
                Type = type,
                Scope = scope
            };
            
            parent.AcquireType(typeRef);
        }

        public override void ExitGeneric_types(KryptonParser.Generic_typesContext context)
        {
            TypeParents.Pop();
        }

        #endregion

        #region Operation Statements

        public override void EnterData_statement(KryptonParser.Data_statementContext context)
        {
            var operation = new TypeStatement
            {
                Name = context.IDENTIFIER().GetText()
            };
            TypeParents.Push(operation);
            
            var container = StatementBlocks.Peek();
            container.AddStatement(operation);
        }

        public override void ExitData_statement(KryptonParser.Data_statementContext context)
        {
            TypeParents.Pop();
        }

        #endregion

        #region Protocols

        public override void EnterProtocol_definition(KryptonParser.Protocol_definitionContext context)
        {
            var protocol = new Protocol
            {
                Name = context.name.Text,
                Namespace = context.ns.GetText()
            };
            ActiveProtocol = protocol;
        }

        public override void ExitProtocol_definition(KryptonParser.Protocol_definitionContext context)
        {
            ActiveProtocol = null;
        }

        public override void EnterMessage_definitions(KryptonParser.Message_definitionsContext context)
        {
            var protocol = ActiveProtocol;
            protocol.AddMessage(context.name.Text);
        }

        #endregion
        
        #region Libraries
        
        public override void EnterLibrary_definition(KryptonParser.Library_definitionContext context)
        {
            var library = new Library
            {
                Name = context.name.Text
            };
            
            File.Libraries.Add(library);
            Customizables.Push(library);
            ActiveLibrary = library;
        }

        public override void ExitLibrary_definition(KryptonParser.Library_definitionContext context)
        {
            Customizables.Pop();
            var library = ActiveLibrary;
            
            // Warn the user if the library wasnt assigned a namespace
            if (library.Namespace == null)
            {
                Console.Out.WriteLine($"WARNING: The library {library.Name} wasn't assigned a namespace, using the default.");
                library.Namespace = "Krypton.LibProtocol.Library";
            }

            ActiveLibrary = null;
        }

        #endregion
        
        public override void EnterPacket_definition(KryptonParser.Packet_definitionContext context)
        {
            var container = new Packet
            {
                Name = context.name.Text
            };
            
            ActiveProtocol.AddPacket(container);
            StatementBlocks.Push(container.Statements);
        }
        
        public override void ExitPacket_definition(KryptonParser.Packet_definitionContext context)
        {
            StatementBlocks.Pop();
        }

        public override void EnterGroup_definition(KryptonParser.Group_definitionContext context)
        {
            var group = new Group
            {
                Name = context.name.Text
            };
            
            File.AddGroup(group);
        }

        public override void EnterMember_option(KryptonParser.Member_optionContext context)
        {
            // todo: support more than string values
            var customizable = Customizables.Peek();

            var key = context.OPTION_KEY().GetText();
            var value = context.option_value().STRING_VAL().GetText();
            value = value.Substring(1, value.Length - 2);
            
            OptionUtil.ApplyOption(customizable, key, value);
        }
    }
}
