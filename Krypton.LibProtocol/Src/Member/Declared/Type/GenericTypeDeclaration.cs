using System.Collections.Generic;
using System.Collections.ObjectModel;
using Krypton.LibProtocol.Member.Type;
using Krypton.LibProtocol.Target;

namespace Krypton.LibProtocol.Member.Declared.Type
{
    public class GenericTypeDeclaration : TypeDeclarationBase, ITemplateType
    {
        public string TemplateName => "generic_typedecl";
        
        public IEnumerable<GenericAttribute> Generics { get; }
        private readonly IList<GenericAttribute> _generics = new List<GenericAttribute>();

        public GenericTypeDeclaration(string name, IMemberContainer parent) : base(name, parent)
        {
            Generics = new ReadOnlyCollection<GenericAttribute>(_generics);
        }

        public void AddGeneric(GenericAttribute generic)
        {
            _generics.Add(generic);
        }
    }
}
