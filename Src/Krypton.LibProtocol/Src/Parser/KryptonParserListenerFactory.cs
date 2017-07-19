using Krypton.LibProtocol.File;

namespace Krypton.LibProtocol.Parser
{
    public class KryptonParserListenerFactory
    {
        public static KryptonParserListenerFactory Instance = new KryptonParserListenerFactory();

        private KryptonParserListenerFactory()
        {
        }

        public KryptonParserListener Create(DefinitionFile file, string filepath)
        {
            var listener = new KryptonParserListener(file);
            listener.BuildRootContainer(filepath);
            return listener;
        }
    }
}