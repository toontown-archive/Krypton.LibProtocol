using System.Collections.Generic;
using Krypton.LibProtocol.Member.Type;

namespace Krypton.LibProtocol.Member.Operation
{
    public class DataOperation : OperationBase, ITypeContainer
    {
        public TypeReference Type { get; private set; }
        public string Name { get; internal set; }
        
        public void AcquireTypeReference(TypeReference reference)
        {
            Type = reference;
        }

        public override IList<DataOperation> Members => new[] {this};
    }
}
