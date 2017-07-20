using Krypton.LibProtocol.Extensions;
using Krypton.LibProtocol.Target;

namespace Krypton.LibProtocol.Member.Type
{
    public class ConcreteType : IType, ITemplateType
    {
        public string TemplateName => "concrete_type";
        
        /// <summary>
        /// The name of the Type
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The name of the Type in CamelCase
        /// </summary>
        public string CamelCaseName => Name.ToCamelCase();

        public ConcreteType(string name)
        {
            Name = name;
        }
    }
}
