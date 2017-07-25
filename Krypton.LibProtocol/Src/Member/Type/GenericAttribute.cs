using Krypton.LibProtocol.Target;

namespace Krypton.LibProtocol.Member.Type
{
    /// <summary>
    /// A GenericAttribute is a parameterized type 
    /// </summary>
    public class GenericAttribute : IType, ITemplateType, INameable
    {
        public string TemplateName => "generic_attribute";
        
        public string Name { get; }
        
        internal GenericAttribute(string name)
        {
            Name = name;
        }
    }
}
