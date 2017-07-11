using System.Collections.Generic;
using Krypton.LibProtocol.Member.Operation;

namespace Krypton.LibProtocol.Member.Type
{
    public class TypeDeclaration : IOperationContainer
    {
        public TypeName Name { get; internal set; }
        public IList<OperationBase> Operations { get; }

        public TypeDeclaration()
        {
            Operations = new List<OperationBase>();
        }

        public void AddOperation(OperationBase operation)
        {
            Operations.Add(operation);
        }
    }
}