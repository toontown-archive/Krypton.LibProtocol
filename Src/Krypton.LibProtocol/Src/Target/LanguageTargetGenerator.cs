using System.IO;
using Antlr4.StringTemplate;

namespace Krypton.LibProtocol.Target
{
    public abstract class LanguageTargetGenerator<TS> 
        where TS: ILanguageTargetSettings
    {
        protected abstract string TemplatesPath { get; }

        protected KPDLFile File { get; }

        protected LanguageTargetGenerator(KPDLFile file)
        {
            File = file;
        }

        public abstract void Generate(TS settings);

        protected void WriteFile(string data, string filepath)
        {
            using (var f = new FileStream(filepath, FileMode.Create))
            {
                using (var s = new StreamWriter(f))
                {
                    s.Write(data);
                }
            }
        }

        public TemplateGroupString ReadTemplate(string template)
        {
            var full = Path.Combine(TemplatesPath, template);
            var resource = full.Replace('/', '.');
            
            var s = new TemplateGroupString(ReadResource(resource));
            s.Load();
            return s;
        }

        public static Stream OpenResource(string path)
        {
            return TargetResources.Assembly.GetManifestResourceStream(path);
        }

        public static string ReadResource(string path)
        {
            var ss = new StringWriter();
            using (var stream = OpenResource(path))
            {
                while (stream.Position < stream.Length)
                    ss.Write((char) stream.ReadByte());
            }
            return ss.ToString();
        }
    }
}
