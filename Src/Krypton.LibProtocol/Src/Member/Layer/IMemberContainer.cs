using System.Collections.Generic;

namespace Krypton.LibProtocol.Member.Layer
{
    public interface IMemberContainer
    {
        IEnumerable<IMember> Members { get; }

        void AddMember(IMember member);
    }
}
