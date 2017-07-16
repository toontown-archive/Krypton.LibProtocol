using Krypton.LibProtocol.Extensions;
using Krypton.LibProtocol.Member.Type;
using Krypton.LibProtocol.Parser;

namespace Krypton.LibProtocol.Member.Statement
{
    public class TypeStatement : IStatement, ITypeParent
    {
        /// <summary>
        /// The name of the type reference
        /// </summary>
        public string Name { get; internal set; }
        
        /// <summary>
        /// The name of the type in CamelCase
        /// </summary>
        public string CamelCaseName => Name.ToCamelCase();
        
        public TypeReference Type { get; internal set; }

        internal TypeStatement()
        {
        }
        
        public string StatementAlias => "type_statement";
        
        public void AcquireType(TypeReference type)
        {
            Type = type;
        }
    }
}
