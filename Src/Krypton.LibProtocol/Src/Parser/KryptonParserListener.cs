using System;
using System.Collections.Generic;
using Krypton.LibProtocol.File;
using Krypton.LibProtocol.Member;
using Krypton.LibProtocol.Member.Common;
using Krypton.LibProtocol.Member.Declared;
using Krypton.LibProtocol.Member.Declared.Type;
using Krypton.LibProtocol.Member.Statement;
using Krypton.LibProtocol.Member.Type;
using Krypton.LibProtocol.Member.Type.Layer;
using Krypton.LibProtocol.Member.Type.Scope;

namespace Krypton.LibProtocol.Parser
{
    public class KryptonParserListener : KryptonParserBaseListener
    {
        private readonly DefinitionFile _file;

        private Protocol _activeProtocol;
        private Library _activeLibrary;
        private readonly Stack<StatementBlock> _statementBlocks;
        private readonly Stack<ICustomizable> _customizables;
        private readonly Stack<ITypeReferenceContainer> _typeParents;

        public KryptonParserListener(DefinitionFile file)
        {
            _file = file;
            _statementBlocks = new Stack<StatementBlock>();
            _typeParents = new Stack<ITypeReferenceContainer>();
            _customizables = new Stack<ICustomizable>();
        }

        public override void EnterImport_statement(KryptonParser.Import_statementContext context)
        {
            var dir = context.path?.GetText() ?? "";
            var filename = context.file.Text;
            var filepath = $"{dir}{filename}.kpdl";
            
            _file.Load(filepath);
        }

        #region Type Declaration

        public override void EnterType_declaration(KryptonParser.Type_declarationContext context)
        {
            DeclaredTypeBase declaredType;
            var name = context.IDENTIFIER().GetText();

            var generics = context.generic_type_attributes()?.IDENTIFIER();
            if (generics != null)
            {
                var genericType = new DeclaredGenericType(name, _activeLibrary);
                foreach (var g in generics)
                {
                    genericType.Generics.Add(g.GetText());
                }
                declaredType = genericType;
            }
            else
            {
                declaredType = new DeclaredType(name, _activeLibrary);
            }

            _activeLibrary.AddType(declaredType);
            _statementBlocks.Push(declaredType.Statements);
        }

        public override void ExitType_declaration(KryptonParser.Type_declarationContext context)
        {
            _statementBlocks.Pop();
        }

        #endregion

        #region Type Reference

        public override void EnterBuiltin_type_reference(KryptonParser.Builtin_type_referenceContext context)
        {
            var parent = _typeParents.Peek();

            var name = context.BUILTIN_TYPE().GetText();
            var scope = new BuiltinScope();
            TypeBase type;
            
            var generics = context.generic_types();
            if (generics == null)
            {
                type = new Member.Type.ConcreteType(name);
            }
            else
            {
                type = new GenericType(name);
                _typeParents.Push((ITypeReferenceContainer)type);
            }

            var typeRef = new DeclaredTypeReference
            {
                Type = type,
                Scope = scope
            };
            
            parent.AddTypeReference(typeRef);
        }

        public override void EnterDeclared_type_reference(KryptonParser.Declared_type_referenceContext context)
        {
            var parent = _typeParents.Peek();
            var name = context.IDENTIFIER().GetText();
            
            // resolve the scope the type is in
            var libName = context.declared_namespace().GetText();
            var library = _file.ResolveLibrary(libName);
            if (library == null)
            {
                throw new KryptonParserException($"Unknown library \"{libName}\"");
            }

            // resolve the type declaration
            var decl = library.ResolveType(name);
            if (decl == null)
            {
                throw new KryptonParserException($"Reference to unknown type \"{name}\"");
            }

            var scope = new DeclaredScope
            {
                Library = library
            };
            TypeBase type;
            
            var generics = context.generic_types();
            if (generics == null)
            {
                type = new Member.Type.ConcreteType(name);
            }
            else
            {
                type = new GenericType(name);
                _typeParents.Push((ITypeReferenceContainer)type);
            }

            var typeRef = new DeclaredTypeReference
            {
                Type = type,
                Scope = scope
            };
            
            parent.AddTypeReference(typeRef);
        }

        public override void EnterLocal_type_reference(KryptonParser.Local_type_referenceContext context)
        {
            var parent = _typeParents.Peek();

            var name = context.IDENTIFIER().GetText();
            var scope = new ActiveScope();
            TypeBase type;
            
            var generics = context.generic_types();
            if (generics == null)
            {
                type = new Member.Type.ConcreteType(name);
            }
            else
            {
                type = new GenericType(name);
                _typeParents.Push((ITypeReferenceContainer)type);
            }
            
            var typeRef = new DeclaredTypeReference
            {
                Type = type,
                Scope = scope
            };
            
            parent.AddTypeReference(typeRef);
        }

        public override void EnterGeneric_attribute_reference(KryptonParser.Generic_attribute_referenceContext context)
        {
            var parent = _typeParents.Peek();

            var name = context.IDENTIFIER().GetText();
            var scope = new ActiveScope();
            var type = new GenericAttribute(name);

            var typeRef = new DeclaredTypeReference
            {
                Type = type,
                Scope = scope
            };

            parent.AddTypeReference(typeRef);
        }

        public override void ExitGeneric_types(KryptonParser.Generic_typesContext context)
        {
            _typeParents.Pop();
        }

        #endregion

        #region Operation Statements

        public override void EnterData_statement(KryptonParser.Data_statementContext context)
        {
            var name = context.IDENTIFIER().GetText();
            var operation = new TypeStatement(name);
            _typeParents.Push(operation);
            
            var container = _statementBlocks.Peek();
            container.AddStatement(operation);
        }

        public override void ExitData_statement(KryptonParser.Data_statementContext context)
        {
            _typeParents.Pop();
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
            _activeProtocol = protocol;
        }

        public override void ExitProtocol_definition(KryptonParser.Protocol_definitionContext context)
        {
            _activeProtocol = null;
        }

        public override void EnterMessage_definitions(KryptonParser.Message_definitionsContext context)
        {
            var protocol = _activeProtocol;
            protocol.AddMessage(context.name.Text);
        }

        #endregion
        
        #region Libraries
        
        public override void EnterLibrary_definition(KryptonParser.Library_definitionContext context)
        {
            var library = new Library(context.name.Text);
            
            // Check to see if this library has already been defined
            var dup = _file.ResolveLibrary(library.Name);
            if (dup != null)
            {
                throw new KryptonParserException($"Duplicate definition of Library \"{library.Name}\".");
            }

            _file.Libraries.Add(library);
            _customizables.Push(library);
            _activeLibrary = library;
        }

        public override void ExitLibrary_definition(KryptonParser.Library_definitionContext context)
        {
            _customizables.Pop();
            var library = _activeLibrary;
            
            // Warn the user if the library wasnt assigned a namespace
            if (library.Namespace == null)
            {
                Console.Out.WriteLine($"WARNING: The library {library.Name} wasn't assigned a namespace, using the default.");
                library.Namespace = "Krypton.LibProtocol.Library";
            }

            _activeLibrary = null;
        }

        #endregion
        
        public override void EnterPacket_definition(KryptonParser.Packet_definitionContext context)
        {
            var container = new Packet
            {
                Name = context.name.Text
            };
            
            _activeProtocol.AddPacket(container);
            _statementBlocks.Push(container.Statements);
        }
        
        public override void ExitPacket_definition(KryptonParser.Packet_definitionContext context)
        {
            _statementBlocks.Pop();
        }

        public override void EnterGroup_definition(KryptonParser.Group_definitionContext context)
        {
            var group = new Group
            {
                Name = context.name.Text
            };
            
            _file.AddGroup(group);
        }

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
