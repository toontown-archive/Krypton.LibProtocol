using Krypton.LibProtocol.Target;

namespace Krypton.LibProtocol.Member.Type
{
    public class FormalTypeReference : ITypeReference, ITemplateType
    {
        public string TemplateName => "formal_typeref";
        
        /// <summary>
        /// Type definition
        /// </summary>
        public IType Type { get; }
        
        /// <summary>
        /// The scope containg the Type
        /// </summary>
        public IMemberContainer Scope { get; }

        internal FormalTypeReference(IType type, IMemberContainer scope)
        {
            Type = type;
            Scope = scope;
        }
    }
}
