using System;
using System.Collections.Generic;

namespace Krypton.LibProtocol.Member.Statement
{
    public interface IStatementContainer
    {
        IEnumerable<IStatement> Statements { get; }

        void AddStatement(IStatement statement);
    }

    public class StatementContainerUtils
    {
        public static IStatement FindStatement(IStatementContainer container, Func<IStatement, bool> filter)
        {
            // Climb through the statement tree if the container has a parent
            if (container is IStatement statement && statement.Parent != null)
            {
                var res = FindStatement(statement.Parent, filter);
                if (res != null)
                {
                    return res;
                }
            }

            // Loop through the container and see if any statements match against the filter
            foreach (var s in container.Statements)
            {
                if (filter(s))
                {
                    return s;
                }

                // Search the statement if its an inner container
                if (!(s is IStatementContainer x)) continue;
                var res = FindStatement(x, filter);
                if (res != null)
                {
                    return res;
                }
            }

            return null;
        }
    }
}
