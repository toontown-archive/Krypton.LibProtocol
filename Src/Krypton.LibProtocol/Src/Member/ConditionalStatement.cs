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

    public class ConditionalStatement : PacketStatement, IStatementContainer
    {
        public Condition Condition { get; }
        public IList<PacketStatement> Statements { get; }

        internal ConditionalStatement(Condition condition)
        {
            Condition = condition;
            Statements = new List<PacketStatement>();
        }
        
        public void AddStatement(PacketStatement statement)
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

        internal void AddStatement(DataStatement statement)
        {
            // todo: check if the name has already been defined
        }

        internal void AddStatement(ConditionalStatement statement)
        {
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
