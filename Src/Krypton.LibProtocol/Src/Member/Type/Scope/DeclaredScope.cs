namespace Krypton.LibProtocol.Member.Type.Scope
{
    public class DeclaredScope : ITypeScope
    {
        /// <summary>
        /// The Library the scope is referring to
        /// </summary>
        public Library Library { get; internal set; }
    }
}
