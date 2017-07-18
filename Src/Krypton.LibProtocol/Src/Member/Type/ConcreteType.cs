using Krypton.LibProtocol.Extensions;
using Krypton.LibProtocol.Member.Type.Layer;

namespace Krypton.LibProtocol.Member.Type
{
    public class ConcreteType : IType
    {
        /// <summary>
        /// The name of the Type
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The name of the Type in CamelCase
        /// </summary>
        public string CamelCaseName => Name.ToCamelCase();

        internal ConcreteType(string name)
        {
            Name = name;
        }
    }
}
