using Krypton.LibProtocol.Target.CSharp;

namespace Krypton.LibProtocol.TestConsole
{
    public class Test
    {
        public static void Main(string[] args)
        {
            var pf = new KryptonFile();
            pf.Read("Resources/groups.krypton");

            var target = new CSharpTarget();
            var context = target.Build(pf);
            
            var settings = new CSharpTargetSettings
            {
                Output = "TestOutput.cs",
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
