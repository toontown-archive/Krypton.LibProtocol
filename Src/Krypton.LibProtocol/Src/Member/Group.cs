using Krypton.LibProtocol.Target;

namespace Krypton.LibProtocol.Member
{
    public class Group : IMember, ITemplateType
    {
        public int Id { get; }
        
        public string Name { get; }
        public IMemberContainer Parent { get; }

        public Group(string name, int id, IMemberContainer parent)
        {
            Name = name;
            Id = id;
            Parent = parent;
        }

        public string TemplateName => "group";
    }
}
