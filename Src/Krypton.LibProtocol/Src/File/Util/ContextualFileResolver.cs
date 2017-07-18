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

        public string Resolve(string path)
        {
            var result = from directory in _directories 
                select Directory.EnumerateFiles(directory, path) into x 
                where x.Any() select x.First();
           
            return result.FirstOrDefault();
        }
    }
}
