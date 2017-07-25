using System;

namespace Krypton.LibProtocol.Parser
{
    public class KryptonParserException : Exception
    {
        public static KryptonParseTreeWalker Walker => KryptonParseTreeWalker.ActiveWalker;
        public static string Header => $"{Walker.Filepath} {Walker.Line}: ";
        
        public KryptonParserException(string msg) : base(Header + msg)
        {
        }
    }
}
