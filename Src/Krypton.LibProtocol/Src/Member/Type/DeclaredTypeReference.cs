namespace Krypton.LibProtocol.Member.Type
{
    public class DeclaredTypeReference : TypeReference
    {
        public string Library { get; internal set; }
        public override string Name { get; internal set; }
        
        public override string TemplateAlias => "declared_type_reference";
    }
}
