using Krypton.LibProtocol.Member.Layer;
using Krypton.LibProtocol.Member.Type.Layer;

namespace Krypton.LibProtocol.Member.Type
{
    public class FormalTypeReference
    {
        /// <summary>
        /// Type definition
        /// </summary>
        public IType Type { get; internal set; }
        
        /// <summary>
        /// The scope containg the Type
        /// </summary>
        public IMemberContainer Scope { get; internal set; }

        internal FormalTypeReference()
        {
        }
    }
}
