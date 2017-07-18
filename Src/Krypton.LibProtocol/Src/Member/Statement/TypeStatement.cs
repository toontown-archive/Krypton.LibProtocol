using Krypton.LibProtocol.Extensions;
using Krypton.LibProtocol.Member.Statement.Layer;
using Krypton.LibProtocol.Member.Type;
using Krypton.LibProtocol.Member.Type.Layer;
using Krypton.LibProtocol.Parser;

namespace Krypton.LibProtocol.Member.Statement
{
    public class TypeStatement : IStatement, ITypeReferenceContainer
    {
        /// <summary>
        /// The name of the type reference
        /// </summary>
        public readonly string Name;
        
        /// <summary>
        /// The name of the type in CamelCase
        /// </summary>
        public string CamelCaseName => Name.ToCamelCase();
        
        public string StatementAlias => "type_statement";
        
        public ITypeReference Type { get; private set; }

        internal TypeStatement(string name)
        {
            Name = name;
        }
        
        public void AddTypeReference(ITypeReference type)
        {
            Type = type;
        }

        public IStatementContainer Parent { get; }
    }
}
