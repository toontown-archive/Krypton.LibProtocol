using System.CodeDom;

namespace Krypton.LibProtocol.Target.CSharp
{
    public class CSharpTargetUnit : ILanguageTargetUnit
    {
        public string Path { get; set; }
        public CodeCompileUnit Unit { get; set; }
    }
}
