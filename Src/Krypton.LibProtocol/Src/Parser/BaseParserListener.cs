﻿using System;
using System.Collections.Generic;
using Krypton.LibProtocol.Member;
using Krypton.LibProtocol.Member.Operation;
using Krypton.LibProtocol.Member.Type;

namespace Krypton.LibProtocol.Parser
{
    public abstract class BaseParserListener : KryptonParserBaseListener
    {
        protected KPDLFile File;

        protected Stack<IPacketContainer> PacketContainers;
        protected Stack<IOperationContainer> OperationContainers;
        protected Stack<ITypeContainer> TypeContainers;
        protected Stack<TypeName> TypeNames;

        protected void Initialize(KPDLFile file)
        {
            File = file;
            PacketContainers = new Stack<IPacketContainer>();
            OperationContainers = new Stack<IOperationContainer>();
            TypeContainers = new Stack<ITypeContainer>();
            TypeNames = new Stack<TypeName>();
        }

        public override void EnterImport_statement(KryptonParser.Import_statementContext context)
        {
            var dir = context.path?.GetText() ?? "";
            var filename = context.file.Text;
            var filepath = $"{dir}{filename}.kpdl";
            
            var ctx = File.GenerateContext(filepath);
            ctx.Build(this);
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

            declaration.Name = typeName;
            parent.AddType(declaration);
            
            TypeNames.Push(typeName);
            OperationContainers.Push(declaration);
        }

        public override void ExitType_declaration(KryptonParser.Type_declarationContext context)
        {
            TypeNames.Pop();
            OperationContainers.Pop();
        }

        public override void EnterGeneric_type_attribute(KryptonParser.Generic_type_attributeContext context)
        {
            var typeName = TypeNames.Peek();
            typeName.Generics.Add(context.IDENTIFIER().GetText());
        }

        #endregion
        
        #region Type Reference

        public override void EnterPrimitive_type_reference(KryptonParser.Primitive_type_referenceContext context)
        {
            var type = new PrimitiveTypeReference
            {
                Type = (Primitive) Enum.Parse(typeof(Primitive), context.PRIMITIVE().GetText(), true)
            };

            var parent = TypeContainers.Peek();
            parent.AcquireTypeReference(type);
        }

        public override void EnterDeclared_type_reference(KryptonParser.Declared_type_referenceContext context)
        {
            var type = new DeclaredTypeReference();
            
            var parent = TypeContainers.Peek();
            parent.AcquireTypeReference(type);
        }

        public override void EnterDeclared_generic_type_reference(KryptonParser.Declared_generic_type_referenceContext context)
        {
            var parent = TypeContainers.Peek();
            var type = new DeclaredGenericTypeReference
            {
                Name = context.IDENTIFIER().GetText()
            };

            parent.AcquireTypeReference(type);
            TypeContainers.Push(type);
        }
        
        public override void ExitDeclared_generic_type_reference(KryptonParser.Declared_generic_type_referenceContext context)
        {
            TypeContainers.Pop();
        }

        public override void EnterPrimitive_generic_type_reference(KryptonParser.Primitive_generic_type_referenceContext context)
        {
            var parent = TypeContainers.Peek();
            var type = new GenericPrimitiveTypeReference
            {
                Type = (Primitive) Enum.Parse(typeof(Primitive), context.PRIMITIVE().GetText(), true)
            };

            parent.AcquireTypeReference(type);
            TypeContainers.Push(type);
        }

        public override void ExitPrimitive_generic_type_reference(KryptonParser.Primitive_generic_type_referenceContext context)
        {
            TypeContainers.Pop();
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
            
            var operation = new IfConditional();
            parent.AddOperation(operation);
            
            OperationContainers.Push(operation);
        }

        public override void ExitIf_statement(KryptonParser.If_statementContext context)
        {
            OperationContainers.Pop();
        }

        public override void EnterLoop_statement(KryptonParser.Loop_statementContext context)
        {
            var parent = OperationContainers.Peek();
            
            var operation = new LoopConditional();
            parent.AddOperation(operation);
            
            OperationContainers.Push(operation);
        }

        public override void ExitLoop_statement(KryptonParser.Loop_statementContext context)
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
            
            PacketContainers.Push(library);
        }

        public override void ExitLibrary_definition(KryptonParser.Library_definitionContext context)
        {
            PacketContainers.Pop();
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
            File.AddGroup(context.name.Text);
        }
    }
}
