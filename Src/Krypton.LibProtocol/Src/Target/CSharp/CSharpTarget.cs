using Proto = Krypton.LibProtocol.Member;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;

namespace Krypton.LibProtocol.Target.CSharp
{
    /// <summary>
    /// CSharp Target Settings
    /// </summary>
    public class CSharpTargetSettings : TargetSettings
    {
        /// <summary>
        /// Output file path
        /// </summary>
        public string Output { get; set; }
    }

    public class CSharpTargetContext : LanguageTargetContext
    {
        private CodeNamespaceCollection _namespaceCollection;
        private CodeTypeDeclaration _groups;
        
        public CSharpTargetContext(KryptonFile file) : base(file)
        {
            _namespaceCollection = new CodeNamespaceCollection();
            
            var ns = TargetUtil.CreateNamespace("Krypton.Protocol");
            _groups = TargetUtil.CreateClass("Groups", ns);
            _namespaceCollection.Add(ns);
        }

        public override void AddGroupDefinition(GroupDefinitionContext context)
        {
            var ctx = (CSharpGroupContext) context;
            _groups.Members.Add(ctx.MemberField);
        }

        public override void AddProtocolDefinition(ProtocolDefinitionContext context)
        {
            throw new System.NotImplementedException();
        }

        public override void Write(TargetSettings settings)
        {
            var s = (CSharpTargetSettings) settings;
            
            var provider = CodeDomProvider.CreateProvider("CSharp");
            var options = new CodeGeneratorOptions
            {
                BracingStyle = "C"
            };

            var ccu = new CodeCompileUnit();
            ccu.Namespaces.AddRange(_namespaceCollection);

            using (var stream = new StreamWriter(s.Output))
            {
                provider.GenerateCodeFromCompileUnit(ccu, stream, options);
            }
        }
    }

    public class CSharpTarget : LanguageTarget
    {
        protected override LanguageTargetContext CreateLanguageTargetContext(KryptonFile file)
        {
            return new CSharpTargetContext(file);
        }

        protected override GroupDefinition CreateGroupDefinition(Proto.Group group)
        {
            var ctx = new CSharpGroupContext(group);
            return new CSharpGroupDefinition(ctx);
        }

        protected override Target.ProtocolDefinition CreateProtocolDefinition(Proto.Protocol protocol)
        {
            throw new System.NotImplementedException();
        }
    }
}
