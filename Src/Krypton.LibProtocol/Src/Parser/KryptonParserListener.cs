using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.StringTemplate.Misc;
using Krypton.LibProtocol.Member;
using Krypton.LibProtocol.Member.Common;
using Krypton.LibProtocol.Member.Operation;
using Krypton.LibProtocol.Member.Type;

namespace Krypton.LibProtocol.Parser
{
    public class KryptonParserListener : KryptonParserBaseListener
    {
        protected KPDLFile File;

        protected Stack<IPacketContainer> PacketContainers;
        protected Stack<IOperationContainer> OperationContainers;
        protected Stack<ITypeContainer> TypeContainers;
        protected Stack<ICustomizable> Customizables;

        public KryptonParserListener(KPDLFile file)
        {
            File = file;
            PacketContainers = new Stack<IPacketContainer>();
            OperationContainers = new Stack<IOperationContainer>();
            TypeContainers = new Stack<ITypeContainer>();
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
            var parent = (Library)PacketContainers.Peek();

            var name = context.IDENTIFIER().GetText();
            var declaration = new TypeDeclaration();
            var typeName = new TypeName
            {
                Name = name
            };

            var generics = context.generic_type_attributes()?.IDENTIFIER();
            if (generics != null)
            {
                foreach (var g in generics)
                {
                    typeName.Generics.Add(g.GetText());
                }
            }

            declaration.Name = typeName;
            parent.AddType(declaration);
            
            OperationContainers.Push(declaration);
        }

        public override void ExitType_declaration(KryptonParser.Type_declarationContext context)
        {
            OperationContainers.Pop();
        }

        #endregion
        
        #region Type Reference

        public override void EnterPrimitive_type_reference(KryptonParser.Primitive_type_referenceContext context)
        {
            var parent = TypeContainers.Peek();
            TypeReference reference;
            
            if (context.generic_types() == null)
            {
                reference = new PrimitiveTypeReference
                {
                    Type = (Primitive) Enum.Parse(typeof(Primitive), context.PRIMITIVE().GetText(), true)
                };
            }
            else
            {
                reference = new GenericPrimitiveTypeReference
                {
                    Type = (GenericPrimitive) Enum.Parse(typeof(GenericPrimitive), context.GENERIC_PRIMITIVE().GetText(), true)
                };
                TypeContainers.Push((ITypeContainer)reference);
            }

            parent.AcquireTypeReference(reference);
        }

        public override void ExitPrimitive_type_reference(KryptonParser.Primitive_type_referenceContext context)
        {
            if (context.generic_types() != null)
            {
                TypeContainers.Pop();
            }
        }
        
        public override void EnterDeclared_type_reference(KryptonParser.Declared_type_referenceContext context)
        {
            var parent = TypeContainers.Peek();
            TypeReference reference;

            if (context.generic_types() == null)
            {
                reference = new DeclaredTypeReference();
            }
            else
            {
                reference = new DeclaredGenericTypeReference
                {
                    Name = context.IDENTIFIER().GetText()
                };
                TypeContainers.Push((ITypeContainer)reference);
            }
            
            parent.AcquireTypeReference(reference);
        }

        public override void ExitDeclared_type_reference(KryptonParser.Declared_type_referenceContext context)
        {
            if (context.generic_types() != null)
            {
                TypeContainers.Pop();
            }
        }

        public override void EnterLocal_type_reference(KryptonParser.Local_type_referenceContext context)
        {
            var parent = TypeContainers.Peek();
            TypeReference reference;

            if (context.generic_types() == null)
            {
                reference = new LocalTypeReference
                {
                    Name = context.IDENTIFIER().GetText()
                };
            }
            else
            {
                reference = new LocalGenericTypeReference
                {
                    Name = context.IDENTIFIER().GetText()
                };
                TypeContainers.Push((ITypeContainer)reference);
            }

            parent.AcquireTypeReference(reference);
        }

        public override void ExitLocal_type_reference(KryptonParser.Local_type_referenceContext context)
        {
            if (context.generic_types() != null)
            {
                TypeContainers.Pop();
            }
        }

        public override void EnterGeneric_attribute_reference(KryptonParser.Generic_attribute_referenceContext context)
        {
            var parent = TypeContainers.Peek();
            var type = new GenericAttributeReference
            {
                Name = context.IDENTIFIER().GetText()
            };

            parent.AcquireTypeReference(type);
        }
        
        #endregion

        #region Operation Statements
        
        public override void EnterData_statement(KryptonParser.Data_statementContext context)
        {
            var operation = new DataOperation
            {
                Name = context.IDENTIFIER().GetText()
            };
            TypeContainers.Push(operation);
            
            var container = OperationContainers.Peek();
            container.AddOperation(operation);
        }

        public override void ExitData_statement(KryptonParser.Data_statementContext context)
        {
            TypeContainers.Pop();
        }

        public override void EnterIf_statement(KryptonParser.If_statementContext context)
        {
            var parent = OperationContainers.Peek();
            
            var operation = new IfOperation();
            parent.AddOperation(operation);
            
            OperationContainers.Push(operation);
        }

        public override void ExitIf_statement(KryptonParser.If_statementContext context)
        {
            OperationContainers.Pop();
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
            PacketContainers.Push(protocol);
        }

        public override void ExitProtocol_definition(KryptonParser.Protocol_definitionContext context)
        {
            PacketContainers.Pop();
        }

        public override void EnterMessage_definitions(KryptonParser.Message_definitionsContext context)
        {
            var parent = (Protocol) PacketContainers.Peek();
            parent.AddMessage(context.name.Text);
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
            PacketContainers.Push(library);
        }

        public override void ExitLibrary_definition(KryptonParser.Library_definitionContext context)
        {
            Customizables.Pop();
            var library = (Library)PacketContainers.Pop();
            
            // Warn the user if the library wasnt assigned a namespace
            if (library.Namespace == null)
            {
                Console.Out.WriteLine($"WARNING: The library {library.Name} wasn't assigned a namespace, using the default.");
                library.Namespace = "Krypton.LibProtocol.Library";
            }
        }

        #endregion
        
        public override void EnterPacket_definition(KryptonParser.Packet_definitionContext context)
        {
            var parent = PacketContainers.Peek();
            var container = new Packet
            {
                Name = context.name.Text
            };
            
            parent.AddPacket(container);
            OperationContainers.Push(container);
        }
        
        public override void ExitPacket_definition(KryptonParser.Packet_definitionContext context)
        {
            OperationContainers.Pop();
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
