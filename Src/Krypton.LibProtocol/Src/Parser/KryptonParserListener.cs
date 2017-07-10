using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Krypton.LibProtocol.Member;
using Krypton.LibProtocol.Member.Operation;
using Krypton.LibProtocol.Member.Type;

namespace Krypton.LibProtocol.Parser
{
    public class KryptonParserListener : KryptonParserBaseListener
    {
        private KPDLFile _file;

        private Stack<IOperationContainer> _operationContainers;
        private Stack<ITypeContainer> _typeContainers;
        private Stack<ITypeNameContainer> _typeNameContainers;

        public KryptonParserListener(KPDLFile file)
        {
            _file = file;
            _operationContainers = new Stack<IOperationContainer>();
            _typeContainers = new Stack<ITypeContainer>();
            _typeNameContainers = new Stack<ITypeNameContainer>();
        }

        #region Type Declaration

        public override void EnterType_declaration(KryptonParser.Type_declarationContext context)
        {
            var declaration = new TypeDeclaration();
            _operationContainers.Push(declaration);
            _typeNameContainers.Push(declaration);
        }

        public override void EnterType_name(KryptonParser.Type_nameContext context)
        {
            var typeName = new TypeName();
            
            var parent = _typeNameContainers.Peek();
            parent.AddTypeName(typeName);
        }

        public override void EnterGeneric_type_name(KryptonParser.Generic_type_nameContext context)
        {
            var typeName = new GenericTypeName();
            
            var parent = _typeNameContainers.Peek();
            parent.AddTypeName(typeName);
            _typeNameContainers.Push(typeName);
        }

        public override void ExitGeneric_type_name(KryptonParser.Generic_type_nameContext context)
        {
            _typeNameContainers.Pop();
        }

        public override void ExitType_declaration(KryptonParser.Type_declarationContext context)
        {
            _operationContainers.Pop();
            _typeNameContainers.Pop();
        }

        #endregion
        
        public override void EnterType_reference(KryptonParser.Type_referenceContext context)
        {
            if (context.declared_type_reference() != null)
            {
                Console.Out.WriteLine("we got a non null");
            }
        }

        public override void EnterDeclared_type_reference(KryptonParser.Declared_type_referenceContext context)
        {
            if (context.declared_generic_type_reference() != null)
            {
                Console.Out.WriteLine("we got a non null 2");
            }
        }

        public override void EnterDeclared_generic_type_reference(KryptonParser.Declared_generic_type_referenceContext context)
        {
            var parent = _typeContainers.Peek();
            var generic = new DeclaredGenericTypeReference();
            
            parent.AcquireTypeReference(generic);
            _typeContainers.Push(generic);
        }

        public override void ExitDeclared_generic_type_reference(KryptonParser.Declared_generic_type_referenceContext context)
        {
            _typeContainers.Pop();
        }

        public override void EnterData_statement(KryptonParser.Data_statementContext context)
        {
            var operation = new DataOperation();
            _typeContainers.Push(operation);
            
            var container = _operationContainers.Peek();
            container.AddOperation(operation);
        }

        public override void ExitData_statement(KryptonParser.Data_statementContext context)
        {
            _typeContainers.Pop();
        }

        public override void EnterPacket_definition(KryptonParser.Packet_definitionContext context)
        {
            var container = new Packet();
            _operationContainers.Push(container);
        }

        public override void ExitPacket_definition(KryptonParser.Packet_definitionContext context)
        {
            _operationContainers.Pop();
        }
    }
}
