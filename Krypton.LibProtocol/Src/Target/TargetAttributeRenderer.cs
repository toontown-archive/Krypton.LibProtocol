using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Antlr4.StringTemplate;
using Antlr4.StringTemplate.Misc;

namespace Krypton.LibProtocol.Target
{
    public class ModelAttribute : Attribute
    {
        public string Name { get; }

        public ModelAttribute(string name)
        {
            Name = name;
        }
    }

    public class TargetModelAdaptor : ObjectModelAdaptor
    {
        public override object GetProperty(Interpreter interpreter, TemplateFrame frame, object o, object property, string propertyName)
        {
            var member = GetType().GetMethods()
                .FirstOrDefault(p => p.GetCustomAttribute<ModelAttribute>()?.Name == propertyName);
            if (member != null)
            {
                return member.Invoke(this, new []{o});
            }

            return base.GetProperty(interpreter, frame, o, property, propertyName);
        }
    }
}