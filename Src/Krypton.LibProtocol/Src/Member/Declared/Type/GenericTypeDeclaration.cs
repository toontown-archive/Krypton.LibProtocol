using System.Collections.Generic;
using System.Collections.ObjectModel;
using Krypton.LibProtocol.Target;

namespace Krypton.LibProtocol.Member.Declared.Type
{
    public class GenericTypeDeclaration : TypeDeclarationBase, ITemplateType
    {
        public string TemplateName => "generic_typedecl";
        
        public IEnumerable<string> Generics { get; }
        private readonly IList<string> _generics = new List<string>();

        public GenericTypeDeclaration(string name, IMemberContainer parent) : base(name, parent)
        {
            Generics = new ReadOnlyCollection<string>(_generics);
        }

        public void AddGeneric(string generic)
        {
            _generics.Add(generic);
        }
    }
}
