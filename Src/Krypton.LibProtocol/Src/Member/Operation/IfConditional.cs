namespace Krypton.LibProtocol.Member.Operation
{
    public class IfConditional : ConditionalOperation
    {
        public enum Operator
        {
            Unknown,
        
            Equality,
            Inequality,
            Less,
            LessOrEqual,
            Greater,
            GreaterOrEqual
        }
        
        public Operator Condition { get; internal set; }
    }
}