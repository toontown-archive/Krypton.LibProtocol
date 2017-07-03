using System;

namespace Krypton.LibProtocol.Parser
{
    public class KryptonParserException : Exception
    {
        /// <summary>
        /// Current context of the parser
        /// </summary>
        internal static int Context { get; set; }
        internal KryptonParserException(string s) : base($"line {Context}: {s}")
        {
        }
    }
}