using Krypton.LibProtocol.Target.CSharp;

namespace Krypton.LibProtocol.TestConsole
{
    public class Test
    {
        public static void Main(string[] args)
        {
            var pf = new KPDLFile();
            pf.Includes.Directories.Add("Resources/");
            pf.Read("example.kpdl");
            
            var settings = new CSharpTargetSettings();
            CSharpTargetGenerator.Generate(pf, settings);
        }
    }
}
