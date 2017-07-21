using Krypton.LibProtocol.Target;

namespace Krypton.LibProtocol.Member.Type
{
    public class ConcreteType : IType, ITemplateType, INameable
    {
        public string TemplateName => "concrete_type";
        
        /// <summary>
        /// The name of the Type
        /// </summary>
        public string Name { get; }

        public ConcreteType(string name)
        {
            Name = name;
        }
    }
}
