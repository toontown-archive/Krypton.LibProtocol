using Krypton.LibProtocol.Member.Common;
using Krypton.LibProtocol.Target;

namespace Krypton.LibProtocol.Member
{
    public class Group : IMember, ITemplateType, INameable, IDocumentable
    {
        public string TemplateName => "group";
        
        public int Id { get; }
        
        public string Name { get; }
        public IMemberContainer Parent { get; }
        
        public Documentation Documentation { get; private set; }

        public Group(string name, int id, IMemberContainer parent)
        {
            Name = name;
            Id = id;
            Parent = parent;
        }

        public void SetDocumentation(Documentation documentation)
        {
            Documentation = documentation;
        }
    }
}
