using Krypton.LibProtocol.Target;

namespace Krypton.LibProtocol.Member.Type
{
    public class BuiltinType : IMember, IType, ITemplateType
    {
        public string TemplateName => "builtin_type";
        
        public string Name { get; }

        public IMemberContainer Parent { get; }

        public BuiltinType(string name, IMemberContainer parent)
        {
            Name = name;
            Parent = parent;
        }
    }
}
