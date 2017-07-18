using Krypton.LibProtocol.File;
using Krypton.LibProtocol.File.Util;
using Krypton.LibProtocol.Target.CSharp;

namespace Krypton.LibProtocol.TestConsole
{
    public class TestConsole
    {
        public static void Main(string[] args)
        {
            var resolver = new ContextualFileResolver();
            resolver.Include("Resources/");
            
            var pf = new DefinitionFile(resolver);
            pf.Load("library_testing.kpdl");
            
            var settings = new CSharpTargetSettings
            {
                OutDirectory = "Gen/"
            };
            var generator = new CSharpGenerator(pf);
            generator.Generate(settings);
        }
    }
}
