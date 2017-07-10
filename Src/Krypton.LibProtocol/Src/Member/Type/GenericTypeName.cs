using System.Collections.Generic;

namespace Krypton.LibProtocol.Member.Type
{
    public class GenericTypeName : TypeName, ITypeNameContainer
    {
        public IList<TypeName> Generics { get; }

        public GenericTypeName()
        {
            Generics = new List<TypeName>();
        }

        public void AddTypeName(TypeName typeName)
        {
            Generics.Add(typeName);
        }
    }
}
