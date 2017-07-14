using System.Collections.Generic;

namespace Krypton.LibProtocol.Member.Operation
{
    public abstract class OperationBase
    {
        public abstract IList<DataOperation> Members { get; }
        
        public abstract string TemplateAlias { get; }

        public string ConsumeAlias => TemplateAlias + "_consume";
        public string WriteAlias => TemplateAlias + "_write";
    }
}
