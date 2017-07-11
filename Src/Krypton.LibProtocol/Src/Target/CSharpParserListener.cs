using Krypton.LibProtocol.Parser;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Text;

namespace Krypton.LibProtocol.Target
{
    public struct CSharpUnit
    {
        public string Path { get; set; }
        public CodeCompileUnit Unit { get; set; }
    }

    public class CSharpParserListener : BaseParserListener
    {
        private IList<CSharpUnit> _units;
        private Stack<CodeTypeDeclaration> _packetContainers;
        private Stack<CodeTypeDeclaration> _operationContainers;
        
        public CSharpParserListener(KPDLFile file) : base(file)
        {
            _units = new List<CSharpUnit>();
            _packetContainers = new Stack<CodeTypeDeclaration>();
            _operationContainers = new Stack<CodeTypeDeclaration>();
        }

        public void WriteUnits()
        {
            var provider = CodeDomProvider.CreateProvider("CSharp");
            var options = new CodeGeneratorOptions
            {
                BracingStyle = "C"
            };
            
            foreach (var unit in _units)
            {
                var path = Path.Combine("Gen/", unit.Path);
                using (var fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    using (var stream = new StreamWriter(fs))
                    {
                        provider.GenerateCodeFromCompileUnit(unit.Unit, stream, options);
                    }
                }
            }
        }

        #region Protocol Definition
        
        public override void EnterProtocol_definition(KryptonParser.Protocol_definitionContext context)
        {
            base.EnterProtocol_definition(context);
            
            var container = new CodeTypeDeclaration
            {
                Name = ActiveProtocol.Name,
                IsClass = true
            };

            var fullname = $"{ActiveProtocol.Namespace}_{ActiveProtocol.Name}";
            var outfile = fullname.Replace('.', '_') + ".cs";
            
            var unit = new CSharpUnit
            {
                Path = outfile,
                Unit = new CodeCompileUnit()
            };
            _units.Add(unit);
                
            _packetContainers.Push(container);
        }

        public override void ExitProtocol_definition(KryptonParser.Protocol_definitionContext context)
        {
            var ns = new CodeNamespace(ActiveProtocol.Namespace);
            ns.Types.Add(_packetContainers.Pop());

            _units[_units.Count-1].Unit.Namespaces.Add(ns);
            
            base.ExitProtocol_definition(context);
        }
        
        #endregion
        
        #region Library Definition
        
        public override void EnterLibrary_definition(KryptonParser.Library_definitionContext context)
        {
            base.EnterLibrary_definition(context);
            
            var container = new CodeTypeDeclaration
            {
                Name = ActiveLibrary.Name,
                IsClass = true
            };

            var fullname = $"krypton_library_{ActiveLibrary.Name}";
            var outfile = fullname.Replace('.', '_') + ".cs";
            
            var unit = new CSharpUnit
            {
                Path = outfile,
                Unit = new CodeCompileUnit()
            };
            _units.Add(unit);
            
            _packetContainers.Push(container);
        }

        public override void ExitLibrary_definition(KryptonParser.Library_definitionContext context)
        {
            var ns = new CodeNamespace("Krypton.Library");
            ns.Types.Add(_packetContainers.Pop());

            _units[_units.Count-1].Unit.Namespaces.Add(ns);
            
            base.ExitLibrary_definition(context);
        }
        
        #endregion

        #region Packet Definition

        public override void EnterPacket_definition(KryptonParser.Packet_definitionContext context)
        {
            base.EnterPacket_definition(context);

            var container = new CodeTypeDeclaration
            {
                Name = ActivePacket.Name,
                IsStruct = true
            };

            var parent = _packetContainers.Peek();
            parent.Members.Add(container);
            
            _operationContainers.Push(container);
        }

        public override void ExitPacket_definition(KryptonParser.Packet_definitionContext context)
        {
            _operationContainers.Pop();
            
            base.ExitPacket_definition(context);
        }

        #endregion

        #region Type Declaration

        public override void EnterType_declaration(KryptonParser.Type_declarationContext context)
        {
            base.EnterType_declaration(context);
            
            var container = new CodeTypeDeclaration
            {
                Name = context.IDENTIFIER().GetText(),
                IsStruct = true
            };
            
            var parent = _packetContainers.Peek();
            parent.Members.Add(container);
            
            _operationContainers.Push(container);
        }

        public override void ExitType_declaration(KryptonParser.Type_declarationContext context)
        {
            _operationContainers.Pop();
            
            base.ExitType_declaration(context);
        }

        public override void EnterGeneric_type_attribute(KryptonParser.Generic_type_attributeContext context)
        {
            base.EnterGeneric_type_attribute(context);

            var container = _operationContainers.Peek();
            container.TypeParameters.Add(
                new CodeTypeParameter(context.IDENTIFIER().GetText())
                );
        }

        #endregion
    }
}