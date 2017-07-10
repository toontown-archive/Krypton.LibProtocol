using System.Collections.Generic;

namespace Krypton.LibProtocol.Member.Operation
{
    public abstract class ConditionalValue
    {
    }

    public abstract class ConditionalOperation : OperationBase, IOperationContainer
    {
        public ConditionalValue Value1 { get; internal set; }
        public ConditionalValue Value2 { get; internal set; }
        
        public IList<OperationBase> Operations { get; }

        protected ConditionalOperation()
        {
            Operations = new List<OperationBase>();
        }

        public void AddOperation(OperationBase operation)
        {
            Operations.Add(operation);
        }
    }
}
