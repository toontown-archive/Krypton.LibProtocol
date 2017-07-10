namespace Krypton.LibProtocol.Parser
{
    public class KryptonParserListener : KryptonParserBaseListener
    {
        private KPDLFile _file;

        public KryptonParserListener(KPDLFile file)
        {
            _file = file;
        }
    }
}
