using System.Collections.Generic;
using System.Collections.ObjectModel;
using Krypton.LibProtocol.Member.Common;
using Krypton.LibProtocol.Member.Layer;

namespace Krypton.LibProtocol.Member
{
    public class Namespace : IMember, IMemberContainer, ICustomizable
    {
        /// <summary>
        /// The namespace containing the Libraries' packets and types
        /// </summary>
        [Option("namespace")]
        public string TargetNamespace { get; internal set; }

        /// <summary>
        /// The alias used to reference the Library inside the KPDL
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// The Parent of the type
        /// </summary>
        public IMemberContainer Parent { get; internal set; }

        public IEnumerable<IMember> Members { get; }
        private readonly IList<IMember> _members = new List<IMember>();

        internal Namespace()
        {
            Members = new ReadOnlyCollection<IMember>(_members);
        }

        public void AddMember(IMember member)
        {
            _members.Add(member);
        }
    }
}
