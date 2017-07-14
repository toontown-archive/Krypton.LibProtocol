using System.Collections.Generic;

namespace Krypton.LibProtocol.Member.Operation
{
    public enum Operator
    {
        Equality,
        Inequality,
        Less,
        LessOrEqual,
        Greater,
        GreaterOrEqual
    }
    
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

        public override IList<DataOperation> Members
        {
            get
            {
                var x = new List<DataOperation>();
                foreach (var operation in Operations)
                {
                    x.AddRange(operation.Members);
                }
                return x;
            }
        }
    }
}
