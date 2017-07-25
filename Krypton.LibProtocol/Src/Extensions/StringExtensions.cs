using System;
using System.Linq;

namespace Krypton.LibProtocol.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string val)
        {
            return val.Split(new [] {"_"}, StringSplitOptions.RemoveEmptyEntries).
                Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1)).
                Aggregate(string.Empty, (s1, s2) => s1 + s2);
        }
    }
}
