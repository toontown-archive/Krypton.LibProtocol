using Krypton.LibProtocol.File;

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
        }
    }
}
