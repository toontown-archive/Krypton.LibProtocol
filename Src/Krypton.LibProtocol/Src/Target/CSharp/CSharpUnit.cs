using System.CodeDom;

namespace Krypton.LibProtocol.Target.CSharp
{
    public struct CSharpUnit
    {
        public string Path { get; set; }
        public CodeCompileUnit Unit { get; set; }
    }
}
