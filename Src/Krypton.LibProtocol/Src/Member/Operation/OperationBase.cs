using System.Collections.Generic;

namespace Krypton.LibProtocol.Member.Operation
{
    public abstract class OperationBase
    {
        public abstract IList<DataOperation> Members { get; }
    }
}
