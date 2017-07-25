using System.Collections.Generic;

namespace Krypton.LibProtocol.Member
{
    public interface IMemberContainer
    {
        /// <summary>
        /// The members defined inside the container
        /// </summary>
        IEnumerable<IMember> Members { get; }
        
        /// <summary>
        /// Adds a new member to the container
        /// </summary>
        /// <param name="member"></param>
        void AddMember(IMember member);

        bool ContainsMember(string name);
        
        bool TryFindMember(IList<string> path, string name, out IMember member);
        bool TryFindMember(string name, out IMember member);
    }
}
