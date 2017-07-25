namespace Krypton.LibProtocol.Member.Common
{
    public class DocumentationFactory
    {
        public static DocumentationFactory Instance = new DocumentationFactory();

        private DocumentationFactory()
        {
        }

        // TODO: create a documentation parser
        public Documentation Create(string text)
        {
            return new Documentation(text);
        }
    }
}