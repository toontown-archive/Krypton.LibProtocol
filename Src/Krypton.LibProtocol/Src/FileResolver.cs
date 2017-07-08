using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Krypton.LibProtocol
{
    public class FileResolver
    {
        public IList<string> Directories { get; }

        public FileResolver()
        {
            Directories = new List<string>
            {
                "./" // current directory
            };
        }

        public string Resolve(string segment)
        {
            var result = from directory in Directories 
                select Directory.EnumerateFiles(directory, segment) into x 
                where x.Any() select x.First();
           
            return result.FirstOrDefault();
        }
    }
}
