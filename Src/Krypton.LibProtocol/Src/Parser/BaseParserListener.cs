using System;
using System.Collections.Generic;
using Krypton.LibProtocol.Member;
using Krypton.LibProtocol.Member.Operation;
using Krypton.LibProtocol.Member.Operation.Meta;
using Krypton.LibProtocol.Member.Type;

namespace Krypton.LibProtocol.Parser
{
    public abstract class BaseParserListener : KryptonParserBaseListener
    {
        private KPDLFile _file;

        private Stack<IPacketContainer> _packetContainers;
        private Stack<IOperationContainer> _operationContainers;
        private Stack<ITypeContainer> _typeContainers;
        private Stack<TypeName> _typeNames;

        protected TypeDeclaration ActiveTypeDeclaration { get; private set; }
        protected TypeReference ActiveTypeReference { get; private set; }
        protected OperationBase ActiveOperation { get; private set; }
        
        protected Protocol ActiveProtocol { get; private set; }
        protected Packet ActivePacket { get; private set; }
        protected Library ActiveLibrary { get; private set; }
        
        protected BaseParserListener(KPDLFile file)
        {
            _file = file;
            _packetContainers = new Stack<IPacketContainer>();
            _operationContainers = new Stack<IOperationContainer>();
            _typeContainers = new Stack<ITypeContainer>();
            _typeNames = new Stack<TypeName>();
        }

        public override void EnterImport_statement(KryptonParser.Import_statementContext context)
        {
            var dir = context.path?.GetText() ?? "";
            var filename = context.file.Text;
            var filepath = $"{dir}{filename}.kpdl";
            _file.Read(filepath);
        }

        #region Type Declaration

        public override void EnterType_declaration(KryptonParser.Type_declarationContext context)
        {
            var parent = (Library)_packetContainers.Peek();

            var name = context.IDENTIFIER().GetText();
            var declaration = new TypeDeclaration();
            var typeName = new TypeName
            {
                Name = name
            };

            declaration.Name = typeName;
            parent.AddType(declaration);
            
            _typeNames.Push(typeName);
            _operationContainers.Push(declaration);

            ActiveTypeDeclaration = declaration;
        }

        public override void ExitType_declaration(KryptonParser.Type_declarationContext context)
        {
            _typeNames.Pop();
            _operationContainers.Pop();
        }

        public override void EnterGeneric_type_attribute(KryptonParser.Generic_type_attributeContext context)
        {
            var typeName = _typeNames.Peek();
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

            var parent = _typeContainers.Peek();
            parent.AcquireTypeReference(type);

            ActiveTypeReference = type;
        }

        public override void EnterDeclared_type_reference(KryptonParser.Declared_type_referenceContext context)
        {
            var type = new DeclaredTypeReference();
            
            var parent = _typeContainers.Peek();
            parent.AcquireTypeReference(type);

            ActiveTypeReference = type;
        }

        public override void EnterDeclared_generic_type_reference(KryptonParser.Declared_generic_type_referenceContext context)
        {
            var parent = _typeContainers.Peek();
            var type = new DeclaredGenericTypeReference
            {
                Name = context.IDENTIFIER().GetText()
            };

            parent.AcquireTypeReference(type);
            _typeContainers.Push(type);

            ActiveTypeReference = type;
        }
        
        public override void ExitDeclared_generic_type_reference(KryptonParser.Declared_generic_type_referenceContext context)
        {
            _typeContainers.Pop();
        }

        public override void EnterGeneric_attribute_reference(KryptonParser.Generic_attribute_referenceContext context)
        {
            var parent = _typeContainers.Peek();
            var type = new GenericAttributeReference
            {
                Name = context.IDENTIFIER().GetText()
            };

            parent.AcquireTypeReference(type);

            ActiveTypeReference = type;
        }

        #endregion

        #region Operation Statements
        
        public override void EnterData_statement(KryptonParser.Data_statementContext context)
        {
            var operation = new DataOperation();
            _typeContainers.Push(operation);
            
            var container = _operationContainers.Peek();
            container.AddOperation(operation);

            ActiveOperation = operation;
        }

        public override void ExitData_statement(KryptonParser.Data_statementContext context)
        {
            _typeContainers.Pop();
        }

        public override void EnterEnumerable_declaration(KryptonParser.Enumerable_declarationContext context)
        {
            var parent = _operationContainers.Peek();

            var operation = new EnumerableOperation();
            parent.AddOperation(operation);
            
            _typeContainers.Push(operation);

            ActiveOperation = operation;
        }

        public override void ExitEnumerable_declaration(KryptonParser.Enumerable_declarationContext context)
        {
            _typeContainers.Pop();
        }

        public override void EnterIf_statement(KryptonParser.If_statementContext context)
        {
            var parent = _operationContainers.Peek();
            
            var operation = new IfConditional();
            parent.AddOperation(operation);
            
            _operationContainers.Push(operation);

            ActiveOperation = operation;
        }

        public override void ExitIf_statement(KryptonParser.If_statementContext context)
        {
            _operationContainers.Pop();
        }

        public override void EnterLoop_statement(KryptonParser.Loop_statementContext context)
        {
            var parent = _operationContainers.Peek();
            
            var operation = new LoopConditional();
            parent.AddOperation(operation);
            
            _operationContainers.Push(operation);

            ActiveOperation = operation;
        }

        public override void ExitLoop_statement(KryptonParser.Loop_statementContext context)
        {
            _operationContainers.Pop();
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
            _packetContainers.Push(protocol);

            ActiveProtocol = protocol;
        }

        public override void ExitProtocol_definition(KryptonParser.Protocol_definitionContext context)
        {
            _packetContainers.Pop();
        }

        public override void EnterMessage_definitions(KryptonParser.Message_definitionsContext context)
        {
            var parent = (Protocol) _packetContainers.Peek();
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
            
            _packetContainers.Push(library);

            ActiveLibrary = library;
        }

        public override void ExitLibrary_definition(KryptonParser.Library_definitionContext context)
        {
            _packetContainers.Pop();
        }

        #endregion
        
        public override void EnterPacket_definition(KryptonParser.Packet_definitionContext context)
        {
            var parent = _packetContainers.Peek();
            var container = new Packet
            {
                Name = context.name.Text
            };
            
            parent.AddPacket(container);
            _operationContainers.Push(container);

            ActivePacket = container;
        }
        
        public override void ExitPacket_definition(KryptonParser.Packet_definitionContext context)
        {
            _operationContainers.Pop();
        }

        public override void EnterGroup_definition(KryptonParser.Group_definitionContext context)
        {
            _file.AddGroup(context.name.Text);
        }
    }
}
