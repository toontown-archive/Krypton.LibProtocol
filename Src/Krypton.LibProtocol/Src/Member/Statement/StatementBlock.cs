using System.Collections.Generic;

namespace Krypton.LibProtocol.Member.Statement
{
    /// <summary>
    /// A StatementBlock is a group of statements
    /// </summary>
    public class StatementBlock
    {
        /// <summary>
        /// The entry level statements under the block
        /// </summary>
        public IList<IStatement> Statements { get; }

        internal StatementBlock()
        {
            Statements = new List<IStatement>();
        }

        /// <summary>
        /// Adds a new statement to the block
        /// </summary>
        /// <param name="statement">The statement</param>
        internal void AddStatement(IStatement statement)
        {
            Statements.Add(statement);
        }
    }
}