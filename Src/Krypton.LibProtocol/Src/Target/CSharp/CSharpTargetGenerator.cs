using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using Krypton.LibProtocol.Member;
using Krypton.LibProtocol.Member.Operation;
using Krypton.LibProtocol.Member.Type;

namespace Krypton.LibProtocol.Target.CSharp
{
    public class CSharpTargetGenerator : LanguageTargetGenerator<CSharpTargetGenerator, CSharpTargetUnit, CSharpTargetSettings>
    {
        private Stack<CodeTypeDeclaration> _packetContainers;
        private Stack<CodeTypeDeclaration> _operationContainers;
        
        protected override void Initialize(KPDLFile file, CSharpTargetSettings settings)
        {
            base.Initialize(file, settings);
            
            _packetContainers = new Stack<CodeTypeDeclaration>();
            _operationContainers = new Stack<CodeTypeDeclaration>();
        }

        #region Protocol Definition
        
        protected override void EnterProtocol(Protocol protocol)
        {
            var container = new CodeTypeDeclaration
            {
                Name = protocol.Name.ToCamelCase(),
                IsClass = true
            };

            var fullname = $"{protocol.Namespace}_{protocol.Name}";
            var outfile = fullname.Replace('.', '_') + ".cs";
            
            var unit = new CSharpTargetUnit
            {
                Path = outfile,
                Unit = new CodeCompileUnit()
            };
            Units.Add(unit);
                
            _packetContainers.Push(container);
        }

        protected override void ExitProtocol(Protocol protocol)
        {
            var ns = new CodeNamespace(protocol.Namespace);
            ns.Types.Add(_packetContainers.Pop());

            Units[Units.Count-1].Unit.Namespaces.Add(ns);
        }

        #endregion
        
        #region Library Definition

        protected override void EnterLibrary(Library library)
        {
            var container = new CodeTypeDeclaration
            {
                Name = library.Name.ToCamelCase(),
                IsClass = true
            };

            var fullname = $"krypton_library_{library.Name}";
            var outfile = fullname.Replace('.', '_') + ".cs";
            
            var unit = new CSharpTargetUnit
            {
                Path = outfile,
                Unit = new CodeCompileUnit()
            };
            Units.Add(unit);
            
            _packetContainers.Push(container);
        }

        protected override void ExitLibrary(Library library)
        {
            var ns = new CodeNamespace("Krypton.Library");
            ns.Types.Add(_packetContainers.Pop());

            Units[Units.Count-1].Unit.Namespaces.Add(ns);
        }

        #endregion

        #region Packet Definition

        protected override void EnterPacket(Packet packet)
        {
            var container = new CodeTypeDeclaration
            {
                Name = packet.Name.ToCamelCase(),
                IsStruct = true
            };

            var parent = _packetContainers.Peek();
            parent.Members.Add(container);
            
            _operationContainers.Push(container);
        }

        protected override void ExitPacket(Packet packet)
        {
            _operationContainers.Pop();
        }

        #endregion

        #region Type Declaration

        protected override void EnterTypeDeclaration(TypeDeclaration declaration)
        {
            var name = declaration.Name.Name.ToCamelCase();
            var container = new CodeTypeDeclaration
            {
                Name = name,
                IsClass = true,
                BaseTypes =
                {
                    new CodeTypeReference("Krypton.LibProtocol.Type.KryptonType")
                    {
                        TypeArguments = { new CodeTypeReference(name) }
                    }
                }
            };

            var consumeMethod = new CodeMemberMethod
            {
                Name = "Consume",
                Attributes = MemberAttributes.Public | MemberAttributes.Override,

                Parameters =
                {
                    new CodeParameterDeclarationExpression("Krypton.LibProtocol.BufferReader", "br")
                }
            };

            var writeMethod = new CodeMemberMethod
            {
                Name = "Write",
                Attributes = MemberAttributes.Public | MemberAttributes.Override,
                
                Parameters =
                {
                    new CodeParameterDeclarationExpression("Krypton.LibProtocol.BufferWriter", "bw")
                }
            };

            container.Members.Add(consumeMethod);
            container.Members.Add(writeMethod);
            
            var parent = _packetContainers.Peek();
            parent.Members.Add(container);
            
            _operationContainers.Push(container);
        }

        protected override void ExitTypeDeclaration(TypeDeclaration declaration)
        {
            _operationContainers.Pop();
        }

        protected override void AcquireGenericTypeAttribute(string attributeName)
        {
            var container = _operationContainers.Peek();
            
            container.TypeParameters.Add(
                new CodeTypeParameter(attributeName)
            );

            container.BaseTypes[0].TypeArguments[0].TypeArguments.Add(
                new CodeTypeReference(attributeName)
            );
        }

        #endregion

        protected override void AcquireDataOperation(DataOperation operation)
        {
            var field = new CodeMemberField
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Name = operation.Name.ToCamelCase(),
                Type = operation.Type.AsTypeReference()
            };
            field.Name += " { get; set; }//";

            var container = _operationContainers.Peek();
            container.Members.Add(field);
        }

        protected override void WriteUnit(CSharpTargetUnit unit)
        {
            var provider = CodeDomProvider.CreateProvider("CSharp");
            var options = new CodeGeneratorOptions
            {
                BracingStyle = "C"
            };
        
            var path = Path.Combine("Gen/", unit.Path);
            using (var fs = new FileStream(path, FileMode.Create))
            {
                using (var stream = new StreamWriter(fs))
                {
                    provider.GenerateCodeFromCompileUnit(unit.Unit, stream, options);
                }
            }
        }
    }
}