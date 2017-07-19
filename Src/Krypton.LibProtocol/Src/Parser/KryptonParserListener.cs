using System.Collections.Generic;
using Krypton.LibProtocol.File;
using Krypton.LibProtocol.Member;
using Krypton.LibProtocol.Member.Common;
using Krypton.LibProtocol.Member.Layer;

namespace Krypton.LibProtocol.Parser
{
    public class KryptonParserListener : KryptonParserBaseListener
    {
        private readonly DefinitionFile _file;
        private readonly Stack<ICustomizable> _customizables;
        private readonly Stack<IMemberContainer> _memberContainers;
        
        
        public KryptonParserListener(DefinitionFile file)
        {
            _file = file;
            _customizables = new Stack<ICustomizable>();
            _memberContainers = new Stack<IMemberContainer>();
        }

        internal void BuildRootContainer(string name)
        {
            var lib = new Namespace(name);
            _customizables.Push(lib);
            _memberContainers.Push(lib);
        }

        public override void EnterImport_statement(KryptonParser.Import_statementContext context)
        {
            var dir = context.directory()?.GetText() ?? "";
            var filename = context.IDENTIFIER().GetText();
            var filepath = $"{dir}{filename}.kpdl";
            
            _file.Load(filepath);
        }

        public override void EnterMember_option(KryptonParser.Member_optionContext context)
        {
            // todo: support more than string values
            var customizable = _customizables.Peek();

            var key = context.OPTION_KEY().GetText();
            var value = context.option_value().STRING_VAL().GetText();
            value = value.Substring(1, value.Length - 2);
            
            OptionUtil.ApplyOption(customizable, key, value);
        }
    }
}
