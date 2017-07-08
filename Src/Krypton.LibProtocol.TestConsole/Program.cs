using Krypton.LibProtocol.Target.CSharp;

namespace Krypton.LibProtocol.TestConsole
{
    public class Test
    {
        public static void Main(string[] args)
        {
            var pf = new KPDLFile();
            pf.AddIncludeDirectory("Resources/");
            pf.Read("example.kpdl");

            var target = new CSharpTarget();
            var context = target.Build(pf);
            
            var settings = new CSharpTargetSettings
            {
                Output = "Gen",
                Groups = new CSharpTargetSettings.GroupSettings
                {
                    Namespace = "Krypton.Protocol",
                    ClassName = "Groups"
                }
            };
            context.Write(settings);
        }
    }
}
