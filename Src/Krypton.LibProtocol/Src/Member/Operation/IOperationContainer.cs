using System.Collections.Generic;

namespace Krypton.LibProtocol.Member.Operation
{
    public interface IOperationContainer
    {
        IList<OperationBase> Operations { get; }
        
        void AddOperation(OperationBase operation);
    }
}