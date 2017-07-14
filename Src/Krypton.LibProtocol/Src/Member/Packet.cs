using System.Collections.Generic;
using Krypton.LibProtocol.Member.Operation;

namespace Krypton.LibProtocol.Member
{
    public class Packet : IOperationContainer
    {
        public IList<Packet> Parents { get; }
        public string Name { get; internal set; }
        
        public IList<OperationBase> Operations { get; }

        public Packet()
        {
            Parents = new List<Packet>();
            Operations = new List<OperationBase>();
        }

        public void AddOperation(OperationBase operation)
        {
            Operations.Add(operation);
        }
    }
}
