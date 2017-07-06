using Proto = Krypton.LibProtocol.Member;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
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
        
        /// <summary>
        /// Group settings
        /// </summary>
        public GroupSettings Groups { get; set; }

        public struct GroupSettings
        {
            public string Namespace { get; set; }
            public string ClassName { get; set; }
        }
    }

    public class CSharpTargetContext : LanguageTargetContext
    {
        private CSharpTarget _target => (CSharpTarget) Target;
        
        private IList<CSharpGroupContext> _groups;
        private IList<CSharpProtocolContext> _protocols;
        
        public CSharpTargetContext(LanguageTarget target, KryptonFile file) : base(target, file)
        {
            _groups = new List<CSharpGroupContext>();
            _protocols = new List<CSharpProtocolContext>();
        }

        public override void AddGroupDefinition(GroupDefinitionContext context)
        {
            _groups.Add((CSharpGroupContext)context);
        }

        public override void AddProtocolDefinition(ProtocolDefinitionContext context)
        {
            _protocols.Add((CSharpProtocolContext)context);
        }

        private void WriteNamespace(CodeNamespace ns, string output)
        {
            var collection = new CodeNamespaceCollection {ns};
            WriteNamespace(collection, output);
        }

        private void WriteNamespace(CodeNamespaceCollection ns, string output)
        {
            var provider = CodeDomProvider.CreateProvider("CSharp");
            var options = new CodeGeneratorOptions
            {
                BracingStyle = "C"
            };

            var ccu = new CodeCompileUnit();
            ccu.Namespaces.AddRange(ns);

            using (var stream = new StreamWriter(output))
            {
                provider.GenerateCodeFromCompileUnit(ccu, stream, options);
            }
        }

        public override void Write(TargetSettings settings)
        {
            var s = (CSharpTargetSettings) settings;

            // write the groups
            var groupsNamespace = TargetUtil.CreateNamespace(s.Groups.Namespace);
            var groupsClass = TargetUtil.CreateClass(s.Groups.ClassName, groupsNamespace);
            foreach (var group in _groups)
            {
                groupsClass.Members.Add(group.MemberField);
            }

            var groupsPath = Path.Combine(s.Output, "Groups.cs");
            WriteNamespace(groupsNamespace, groupsPath);
            
            // write each protocol
            foreach (var protocol in _protocols)
            {
                var protocolPath = Path.Combine(s.Output, protocol.Outfile);
                WriteNamespace(protocol.Namespace, protocolPath);
            }
            
            // write each resource
            var resources = _target.Resources.GetResources();
            foreach (var resource in resources)
            {
                var resourcePath = Path.Combine(s.Output, resource.Path, resource.Name);
                resource.Write(resourcePath);
            }
        }
    }

    /// <summary>
    /// CSharp Language Target
    /// </summary>
    public class CSharpTarget : LanguageTarget
    {
        public override TargetResources Resources { get; }
        
        public CSharpTarget()
        {
            Resources = new TargetResources("CSharp");
        }

        protected override LanguageTargetContext CreateLanguageTargetContext(KryptonFile file)
        {
            return new CSharpTargetContext(this, file);
        }

        protected override GroupDefinition CreateGroupDefinition(Proto.Group group)
        {
            var ctx = new CSharpGroupContext(group);
            return new CSharpGroupDefinition(ctx);
        }

        protected override ProtocolDefinition CreateProtocolDefinition(Proto.Protocol protocol)
        {
            var ctx = new CSharpProtocolContext(protocol);
            return new CSharpProtocolDefinition(ctx);
        }
    }
}
