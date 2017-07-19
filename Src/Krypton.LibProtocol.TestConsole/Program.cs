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

            // Create a new definition file with a file resolver pointing to Resources/
            var pf = new DefinitionFile
            {
                Resolver = resolver
            };
            pf.PopulateBuiltins();
            pf.Load("example.kpdl");
            
            var settings = new CSharpTargetSettings
            {
                OutDirectory = "Gen/"
            };
            var generator = new CSharpGenerator(pf);
            generator.Generate(settings);
        }
    }
}
