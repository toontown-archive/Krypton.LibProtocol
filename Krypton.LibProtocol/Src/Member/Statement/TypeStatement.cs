using Krypton.LibProtocol.Member.Type;
using Krypton.LibProtocol.Target;

namespace Krypton.LibProtocol.Member.Statement
{
    public class TypeStatement : IStatement, ITypeReferenceContainer, INameable, ITemplateType
    {
        public string TemplateName => "type_statement";
        
        /// <summary>
        /// The name of the type reference
        /// </summary>
        public string Name { get; }
        
        public ITypeReference Type { get; private set; }

        internal TypeStatement(string name, IStatementContainer parent)
        {
            Name = name;
            Parent = parent;
        }
        
        public void AddTypeReference(ITypeReference type)
        {
            Type = type;
        }

        public IStatementContainer Parent { get; }
    }
}
