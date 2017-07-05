using System.Collections.Generic;

namespace Krypton.LibProtocol.Member
{
    /// <summary>
    /// Conditional operator
    /// </summary>
    public enum Operator
    {
        Unknown,
        
        Equal,
        NotEqual,
        Less,
        LessOrEqual,
        Greater,
        GreaterOrEqual
    }

    /// <summary>
    /// Condition
    /// </summary>
    public class Condition
    {
        public string Val1 { get; internal set; }
        public Operator Op { get; internal set; }
        public string Val2 { get; internal set; }
    }

    public class ConditionalStatement : StatementContainer, IPacketStatement 
    {
        public Condition Condition { get; }

        internal ConditionalStatement(Condition condition)
        {
            Condition = condition;
        }
        
        public override void AddStatement(IPacketStatement statement)
        {
            var data = statement as DataStatement;
            if (data != null)
            {
                AddStatement(data);
            }
            else
            {
                AddStatement(statement as ConditionalStatement);
            }

        }

        private void AddStatement(DataStatement statement)
        {
            // todo: checks
            Statements.Add(statement);
        }

        private void AddStatement(ConditionalStatement statement)
        {
            // todo: checks
            Statements.Add(statement);
        }

        public static Operator OperatorFromString(string s)
        {
            switch (s)
            {
                case "==":
                    return Operator.Equal;
                case "!=":
                    return Operator.NotEqual;
                case ">=":
                    return Operator.GreaterOrEqual;
                case ">":
                    return Operator.Greater;
                case "<=":
                    return Operator.LessOrEqual;
                case "<":
                    return Operator.Less;
                default:
                    return Operator.Unknown;
            }
        }
    }
}
