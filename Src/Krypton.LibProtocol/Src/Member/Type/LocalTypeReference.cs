namespace Krypton.LibProtocol.Member.Type
{
    public class LocalTypeReference : TypeReference
    {
        public override string Name { get; internal set; }
    
        public override string TemplateAlias => "local_type_reference";
    }
}
