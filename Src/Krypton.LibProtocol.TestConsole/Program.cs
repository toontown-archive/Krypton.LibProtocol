using Krypton.LibProtocol.Target.CSharp;

namespace Krypton.LibProtocol.TestConsole
{
    public class Test
    {
        public static void Main(string[] args)
        {
            var pf = new KPDLFile();
            pf.Includes.Directories.Add("Resources/");
            pf.Load("library_testing.kpdl");
            
            var settings = new CSharpTargetSettings
            {
                OutDirectory = "Gen/"
            };
            var generator = new CSharpTargetGenerator(pf);
            generator.Generate(settings);
        }
    }
}
