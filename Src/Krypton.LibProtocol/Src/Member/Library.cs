using Krypton.LibProtocol.Member.Common;
using Krypton.LibProtocol.Target;

namespace Krypton.LibProtocol.Member
{
    public class Library : NestedMemberContainer, IMember, ICustomizable, ITemplateType
    {
        public string TemplateName => "library";
        
        /// <summary>
        /// The target output namespace
        /// </summary>
        [Option("namespace")]
        public string TargetNamespace { get; internal set; }

        /// <summary>
        /// The alias used to reference the library inside the KPDL
        /// </summary>
        public string Name { get; }
        
        internal Library(string name, IMemberContainer parent) : base(parent)
        {
            Name = name;
        }
    }
}
