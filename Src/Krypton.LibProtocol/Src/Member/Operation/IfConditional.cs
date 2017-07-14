namespace Krypton.LibProtocol.Member.Operation
{
    public class IfOperation : ConditionalOperation
    {   
        public Operator Condition { get; internal set; }
        public override string TemplateAlias => "if_operation";
    }
}