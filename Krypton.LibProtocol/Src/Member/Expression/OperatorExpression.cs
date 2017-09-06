using System;
using Krypton.LibProtocol.Target;

namespace Krypton.LibProtocol.Member.Expression
{
    public class OperatorExpression : IExpression, ITemplateType
    {
        /// <summary>
        /// Operators, in their numerical form, that can be used within a group of <c>IExpression</c>s.
        /// </summary>
        public enum Operator
        {
            Addition = '+' << 16,                  // +
            Subtraction = '-' << 16,               // -
            Multiplacation = '*' << 16,            // *
            Division = '/' << 16,                  // /
            Modulus = '%' << 16,                   // %
        
            BitwiseAnd = '&' << 16,                // &
            BitwiseOr = '|' << 16,                 // |
            BitwiseXor = '^' << 16,                // ^
            BitwiseNegate = '~' << 16,             // ~
            BitwiseLeftShift = ('<' << 16) | '<',  // <<
            BitwiseRightShift = ('>' << 16) | '>', // >>
        
            And = ('&' << 16) | '&',               // &&
            Or = ('|' << 16) | '|',                // ||
            Equality = ('=' << 16) | '=',          // ==
            Inequality = ('!' << 16) | '=',        // !=
            Greater = '>' << 16,                   // >
            GreaterOrEqual = ('>' << 16) | '=',    // >=
            Less = '<' << 16,                      // <
            LessOrEqual =  ('<' << 16) | '='       // <=
        }
        
        public string TemplateName => "operator_expression";
        
        public Operator Type { get; }

        public OperatorExpression(Operator type)
        {
            Type = type;
        }

        public static explicit operator OperatorExpression(string s)
        {
            if (s.Length > 2 || s.Length == 0)
            {
                throw new ArgumentException("Invalid operator length, must be either 1 or 2.");
            }

            // Ensure we have two chars in our array
            var chars = s.ToCharArray();
            Array.Resize(ref chars, 2);
            
            var numericalVal = (chars[0] << 16) | chars[1];
            return new OperatorExpression((Operator) numericalVal);
        }
    }
}
