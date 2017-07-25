using System;
using System.Linq;
using System.Reflection;
using Krypton.LibProtocol.Parser;

namespace Krypton.LibProtocol.Member.Common
{
    public class OptionAttribute : Attribute 
    {
        /// <summary>
        /// The name of the option
        /// </summary>
        public readonly string Name;
        
        internal OptionAttribute(string name)
        {
            Name = name;
        }
    }

    public static class OptionUtil
    {
        public static void ApplyOption(ICustomizable customizable, string name, object value)
        {
            var property = customizable.GetType().GetProperties()
                    .FirstOrDefault(p => p.GetCustomAttribute<OptionAttribute>()?.Name == name);

            if (property == null)
            {
                throw new KryptonParserException($"Unknown option \"{name}\"");
            }
            
            try
            {
                property.SetValue(customizable, value);
            }
            catch
            {
                throw new KryptonParserException($"Invalid value \"{value}\" for option \"{name}\"");
            }
        }
    }
}
