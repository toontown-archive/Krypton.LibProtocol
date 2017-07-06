using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Krypton.LibProtocol.Target
{
    public struct Resource
    {
        private static readonly Assembly _assembly = Assembly.GetExecutingAssembly();
        
        public string BasePath { get; internal set; }
        
        /// <summary>
        /// The full name of the resource
        /// </summary>
        public string FullName { get; internal set; }

        /// <summary>
        /// The path to the resource
        /// </summary>
        public string Path
        {
            get
            {
                // remove the base path
                var home = FullName.Remove(0, BasePath.Length + 1);

                // remove the file
                var path = home.Substring(0, home.Length - Name.Length);
                return path.Replace(".", "/");
            }
        }

        /// <summary>
        /// The name of the resource
        /// </summary>
        public string Name {
            get
            {
                var count = FullName.Count(c => c == '.');
                
                var nameIndex = 0;
                for (var i = 0; i < count - 1; i++)
                {
                    nameIndex = FullName.IndexOf('.', nameIndex) + 1;
                }

                var name = FullName.Remove(0, nameIndex);
                return name;
            }
        }

        public Stream Open()
        {
            return _assembly.GetManifestResourceStream(FullName);
        }
        
        public void Write(string path)
        {
            using (var stream = Open())
            {
                using (var output = new FileStream(path, FileMode.Create))
                {
                    while (stream.Position < stream.Length)
                    {
                        output.WriteByte((byte)stream.ReadByte());
                    }
                }
            }
        }
    }

    public class TargetResources
    {
        /// <summary>
        /// Assembly containing the resources
        /// </summary>
        protected static Assembly Assembly = Assembly.GetExecutingAssembly();
        
        /// <summary>
        /// Target resource folder name
        /// </summary>
        public string Home { get; }
        
        /// <summary>
        /// Base path containing each Target and their resources
        /// </summary>
        public string BasePath { get; }
        
        /// <summary>
        /// Full path to the Target's resources
        /// </summary>
        public string FullPath => $"{BasePath}.{Home}";
        
        public TargetResources(string home)
        {
            Home = home;
            
            var name = Assembly.GetName().Name;
            BasePath = $"{name}.Resources.Target";
        }

        /// <summary>
        /// Gets a list of the Target's resources
        /// </summary>
        /// <returns></returns>
        public IList<Resource> GetResources()
        {
            var all = Assembly.GetManifestResourceNames();
            var resources = all.Where(s => s.StartsWith(FullPath)).
                Select(s => new Resource
                {
                    BasePath = FullPath,
                    FullName = s
                }).ToList();
            return resources;
        }
    }
}
