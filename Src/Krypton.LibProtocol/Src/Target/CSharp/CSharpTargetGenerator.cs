using System.IO;

namespace Krypton.LibProtocol.Target.CSharp
{
    public class CSharpTargetGenerator : LanguageTargetGenerator<CSharpTargetSettings>
    {
        protected override string TemplatesPath => "Krypton/LibProtocol/Templates/CSharp/";
        
        public CSharpTargetGenerator(KPDLFile file) : base(file)
        {
        }

        protected void GenerateLibraries(CSharpTargetSettings settings)
        {
            var template = ReadTemplate("csharp_target.stg");
            
            foreach (var library in File.Libraries)
            {
                var libdecl = template.GetInstanceOf("library_declaration");
                libdecl.Add("library", library);
                var render = libdecl.Render();
                
                var path = Path.Combine(settings.OutDirectory, $"{library.Name}.cs");
                System.IO.File.WriteAllText(path, render);
            }
        }

        protected void GenerateGroups(CSharpTargetSettings settings)
        {
            var template = ReadTemplate("groups.stg");
            var decl = template.GetInstanceOf("group_declarations");
            decl.Add("kpdl", File);
            var render = decl.Render();
                
            var path = Path.Combine(settings.OutDirectory, "Group.cs");
            System.IO.File.WriteAllText(path, render);
        }

        public override void Generate(CSharpTargetSettings settings)
        {       
            GenerateLibraries(settings);
            GenerateGroups(settings);
        }
    }
}