namespace Krypton.LibProtocol.Member.Common
{
    public interface IDocumentable
    {
        Documentation Documentation { get; }

        void SetDocumentation(Documentation documentation);
    }
}
