using System.Collections.Generic;
using Krypton.LibProtocol.Member.Operation;

namespace Krypton.LibProtocol.Member.Type
{
    public class TypeDeclaration : IOperationContainer
    {
        public TypeName Name { get; internal set; }
        public IList<OperationBase> Operations { get; }

        internal TypeDeclaration()
        {
            Operations = new List<OperationBase>();
        }

        public void AddOperation(OperationBase operation)
        {
            Operations.Add(operation);
        }

        public IList<DataOperation> Members
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
