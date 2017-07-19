namespace Krypton.LibProtocol.Member.Type
{
    /// <summary>
    /// A GenericAttribute is a parameterized type 
    /// </summary>
    public class GenericAttribute : IType
    {
        public string Name { get; }
        
        internal GenericAttribute(string name)
        {
            Name = name;
        }
    }
}
