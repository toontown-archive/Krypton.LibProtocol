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

        public override void Write(TargetSettings settings)
        {
            var s = (CSharpTargetSettings) settings;
            
            var namespaceCollection = new CodeNamespaceCollection();
            
            // create the groups
            var groupsNamespace = TargetUtil.CreateNamespace(s.Groups.Namespace);
            var groupsClass = TargetUtil.CreateClass(s.Groups.ClassName, groupsNamespace);
            foreach (var group in _groups)
            {
                groupsClass.Members.Add(group.MemberField);
            }
            namespaceCollection.Add(groupsNamespace);
            
            // add each protocol
            foreach (var protocol in _protocols)
            {
                namespaceCollection.Add(protocol.Namespace);
            }

            // generate the code
            var provider = CodeDomProvider.CreateProvider("CSharp");
            var options = new CodeGeneratorOptions
            {
                BracingStyle = "C"
            };

            var ccu = new CodeCompileUnit();
            ccu.Namespaces.AddRange(namespaceCollection);

            using (var stream = new StreamWriter(s.Output))
            {
                provider.GenerateCodeFromCompileUnit(ccu, stream, options);
            }
        }
    }

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
