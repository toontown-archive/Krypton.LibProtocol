using System.Collections.Generic;
using System.Collections.ObjectModel;
using Krypton.LibProtocol.Parser;

namespace Krypton.LibProtocol.Member.Type
{
    public class GenericType : IType, IGenericType, ITypeReferenceContainer
    {
        public string Name { get; }
        public IEnumerable<ITypeReference> Generics { get; }
        
        private readonly IList<ITypeReference> _generics = new List<ITypeReference>();

        internal GenericType(string name)
        {
            Name = name;
            Generics = new ReadOnlyCollection<ITypeReference>(_generics);
        }
        
        public void AddTypeReference(ITypeReference type)
        {
            _generics.Add(type);
        }
    }
}
