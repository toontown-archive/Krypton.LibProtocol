using Krypton.LibProtocol.Target;

namespace Krypton.LibProtocol.Member.Declared.Type
{
    public class TypeDeclaration : TypeDeclarationBase, ITemplateType
    {
        public string TemplateName => "typedecl";
        
        internal TypeDeclaration(string name, IMemberContainer parent) : base(name, parent)
        {
        }
    }
}
