using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Krypton.LibProtocol.Member
{
    public abstract class NestedMemberContainer : IMemberContainer
    {
        public IMemberContainer Parent { get; }
        
        public IEnumerable<IMember> Members { get; }
        
        protected IList<IMember> MemberList = new List<IMember>();

        protected NestedMemberContainer(IMemberContainer parent)
        {
            Parent = parent;
            Members = new ReadOnlyCollection<IMember>(MemberList);
        }

        public void AddMember(IMember member)
        {
            MemberList.Add(member);
        }

        public bool ContainsMember(string name)
        {
            return MemberList.Any(m => m.Name == name);
        }

        public bool TryFindMember(string name, out IMember member)
        {
            member = MemberList.FirstOrDefault(m => m.Name == name);
            return member != null;
        }

        public bool TryFindMember(IList<string> path, string name, out IMember member)
        {
            if (path.Count == 0)
                return TryFindMember(name, out member);

            // pop the next container
            var next = path[0];
            
            // create the new path (we dont want to modify the path parameter reference)
            var newPath = path.ToList();
            newPath.RemoveAt(0);

            if (!TryFindMember(next, out member)) 
                return false;
            
            var container = member as IMemberContainer;
            return container != null && container.TryFindMember(newPath, name, out member);
        }
    }
}