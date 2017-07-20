using System.Collections.Generic;
using System.Collections.ObjectModel;
using Krypton.LibProtocol.Member.Common;
using Krypton.LibProtocol.Member.Statement;
using Krypton.LibProtocol.Target;

namespace Krypton.LibProtocol.Member.Declared
{
    public class Packet : NestedMemberContainer, IMember, IStatementContainer, ICustomizable, ITemplateType
    {
        public string TemplateName => "packet";
        
        public string Name { get; }

        public IEnumerable<IStatement> Statements { get; }
        private readonly IList<IStatement> _statements = new List<IStatement>();
        
        public Packet(string name, IMemberContainer parent) : base(parent)
        {
            Name = name;
            Statements = new ReadOnlyCollection<IStatement>(_statements);
        }

        public void AddStatement(IStatement statement)
        {
            _statements.Add(statement);
        }
    }
}
