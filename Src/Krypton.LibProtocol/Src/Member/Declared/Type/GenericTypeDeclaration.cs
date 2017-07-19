using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Krypton.LibProtocol.Member.Declared.Type
{
    public class GenericTypeDeclaration : TypeDeclarationBase
    {
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
