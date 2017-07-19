using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Krypton.LibProtocol.File.Util
{
    public class ContextualFileResolver : IFileResolver
    {
        private readonly IList<string> _directories;

        public ContextualFileResolver()
        {
            _directories = new List<string>
            {
                "./" // current directory
            };
        }

        public void Include(string path)
        {
            _directories.Add(path);
        }

        public bool TryResolve(string path, out string result)
        {
            var results = from directory in _directories 
                select Directory.EnumerateFiles(directory, path) into x 
                where x.Any() select x.First();
            result = results.FirstOrDefault();

            return result != null;
        }
    }
}
