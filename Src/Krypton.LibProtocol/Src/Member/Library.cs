using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Krypton.LibProtocol.Member.Common;
using Krypton.LibProtocol.Member.Declared;
using Krypton.LibProtocol.Member.Declared.Type;
using Krypton.LibProtocol.Member.Layer;
using Krypton.LibProtocol.Member.Type;

namespace Krypton.LibProtocol.Member
{
    public class Library : IMember, IMemberContainer, ICustomizable
    {
        /// <summary>
        /// The namespace containing the Libraries' packets and types
        /// </summary>
        [Option("namespace")]
        public string Namespace { get; internal set; }

        /// <summary>
        /// The alias used to reference the Library inside the KPDL
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The Parent of the type
        /// </summary>
        public IMemberContainer Parent { get; internal set; }

        public IEnumerable<IMember> Members { get; }
        private readonly IList<IMember> _members;

        internal Library(string name)
        {
            Name = name;
            _members = new List<IMember>();
            Members = new ReadOnlyCollection<IMember>(_members);
        }

        public void AddMember(IMember member)
        {
            _members.Add(member);
        }
    }
}
