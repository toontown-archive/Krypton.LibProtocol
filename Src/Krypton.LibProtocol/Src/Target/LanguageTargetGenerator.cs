using System.Collections.Generic;
using Antlr4.Runtime.Tree;
using Krypton.LibProtocol.Member;
using Krypton.LibProtocol.Member.Operation;
using Krypton.LibProtocol.Member.Type;
using Krypton.LibProtocol.Parser;

namespace Krypton.LibProtocol.Target
{
    public abstract class LanguageTargetGenerator<T> : BaseParserListener 
        where T: LanguageTargetGenerator<T>, new()
    {
        protected IList<ILanguageTargetUnit> Units { get; set; }
        
        protected ILanguageTargetSettings Settings { get; set; }
        
        protected virtual void Initialize(KPDLFile file, ILanguageTargetSettings settings)
        {
            base.Initialize(file);
            
            Settings = settings;
            Units = new List<ILanguageTargetUnit>();
        }

        public override void EnterProtocol_definition(KryptonParser.Protocol_definitionContext context)
        {
            base.EnterProtocol_definition(context);

            var protocol = (Protocol)PacketContainers.Peek();
            EnterProtocol(protocol);
        }

        public override void ExitProtocol_definition(KryptonParser.Protocol_definitionContext context)
        {
            var protocol = (Protocol)PacketContainers.Peek();
            ExitProtocol(protocol);
            
            base.ExitProtocol_definition(context);
        }

        protected abstract void EnterProtocol(Protocol protocol);
        
        protected abstract void ExitProtocol(Protocol protocol);

        public override void EnterLibrary_definition(KryptonParser.Library_definitionContext context)
        {
            base.EnterLibrary_definition(context);

            var library = (Library) PacketContainers.Peek();
            EnterLibrary(library);
        }

        public override void ExitLibrary_definition(KryptonParser.Library_definitionContext context)
        {
            var library = (Library) PacketContainers.Peek();
            ExitLibrary(library);
            
            base.ExitLibrary_definition(context);
        }

        protected abstract void EnterLibrary(Library library);
        
        protected abstract void ExitLibrary(Library library);

        public override void EnterPacket_definition(KryptonParser.Packet_definitionContext context)
        {
            base.EnterPacket_definition(context);

            var packet = (Packet) OperationContainers.Peek();
            EnterPacket(packet);
        }

        public override void ExitPacket_definition(KryptonParser.Packet_definitionContext context)
        {
            var packet = (Packet) OperationContainers.Peek();
            ExitPacket(packet);
            
            base.ExitPacket_definition(context);
        }

        protected abstract void EnterPacket(Packet packet);
        
        protected abstract void ExitPacket(Packet packet);
        
        public override void EnterType_declaration(KryptonParser.Type_declarationContext context)
        {
            base.EnterType_declaration(context);

            var declaration = (TypeDeclaration)OperationContainers.Peek();
            EnterTypeDeclaration(declaration);
        }

        public override void ExitType_declaration(KryptonParser.Type_declarationContext context)
        {
            var declaration = (TypeDeclaration)OperationContainers.Peek();
            ExitTypeDeclaration(declaration);
            
            base.ExitType_declaration(context);
        }

        protected abstract void EnterTypeDeclaration(TypeDeclaration declaration);
        
        protected abstract void ExitTypeDeclaration(TypeDeclaration declaration);

        public override void EnterGeneric_type_attribute(KryptonParser.Generic_type_attributeContext context)
        {
            base.EnterGeneric_type_attribute(context);

            AcquireGenericTypeAttribute(context.IDENTIFIER().GetText());
        }

        protected abstract void AcquireGenericTypeAttribute(string attributeName);

        public override void ExitData_statement(KryptonParser.Data_statementContext context)
        {
            var operation = (DataOperation)TypeContainers.Peek();
            AcquireDataOperation(operation);
            
            base.ExitData_statement(context);
        }

        protected abstract void AcquireDataOperation(DataOperation operation);

        protected abstract void WriteUnit(ILanguageTargetUnit unit);
        
        public static void Generate(KPDLFile file, ILanguageTargetSettings settings)
        {
            var generator = new T();
            generator.Initialize(file, settings);

            foreach (var context in file.Contexts)
            {
                var walker = new ParseTreeWalker();
                walker.Walk(generator, context.Context);
            }

            foreach (var unit in generator.Units)
            {
                generator.WriteUnit(unit);
            }
        }
    }
}
