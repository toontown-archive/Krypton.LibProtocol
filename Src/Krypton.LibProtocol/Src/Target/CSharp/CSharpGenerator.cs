using System;
using System.IO;
using Antlr4.Runtime;
using Krypton.LibProtocol.File;
using Krypton.LibProtocol.Member;
using Krypton.LibProtocol.Member.Type;

namespace Krypton.LibProtocol.Target.CSharp
{
    public class CSharpGenerator : LanguageGenerator<CSharpTargetSettings>
    {
        protected override string TemplatesPath => "Krypton/LibProtocol/Templates/";
        
        public CSharpGenerator(DefinitionFile file) : base(file)
        {
        }
        
        public override void Generate(CSharpTargetSettings settings)
        {       
            var template = ReadTemplate("csharp.stg");
            
            // each root member gets its own file
            foreach (var member in File.Members)
            {
                // skip builtin types
                if (member is BuiltinType)
                {
                    continue;
                }

                var targetdecl = template.GetInstanceOf("init");
                targetdecl.Add("root", member);
                var render = targetdecl.Render();
                
                var path = Path.Combine(settings.OutDirectory, $"{member.Name}.cs");
                System.IO.File.WriteAllText(path, render);
            } 
        }
    }
}
