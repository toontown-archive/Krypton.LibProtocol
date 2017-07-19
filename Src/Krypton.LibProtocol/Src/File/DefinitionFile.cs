using System.IO;
using Antlr4.Runtime;
using Krypton.LibProtocol.Member;
using Krypton.LibProtocol.Member.Type;
using Krypton.LibProtocol.Parser;

namespace Krypton.LibProtocol.File
{
    public class DefinitionFile : NestedMemberContainer
    {
        public IFileResolver Resolver { get; set; }
        public readonly IncrementalMessageFactory MessageFactory = new IncrementalMessageFactory();
        public readonly IncrementalGroupFactory GroupFactory = new IncrementalGroupFactory();

        public DefinitionFile() : base(null)
        {
        }
        
        /// <summary> 
        /// Loads in .kpdl file
        /// </summary> 
        /// <param name="filepath">Path to the kpdl file</param> 
        public void Load(string filepath) 
        { 
            filepath = Resolver?.Resolve(filepath) ?? filepath; 
            using (var fs = new FileStream(filepath, FileMode.Open)) 
            { 
                Load(filepath, fs);
            }
        }

        /// <summary>
        /// Loads in a .kpdl file using a resolver from outside the instance
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="resolver"></param>
        public void Load(string filepath, IFileResolver resolver)
        {
            filepath = resolver.Resolve(filepath); 
            Load(filepath);
        }

        /// <summary>
        /// Loads a kpdl file from a stream
        /// </summary>
        /// <param name="name">Name of the kpdl</param>
        /// <param name="stream"></param>
        /// <exception cref="KryptonParserException"></exception>
        public void Load(string name, Stream stream)
        {
            var inputStream = new AntlrInputStream(stream);
            var lexer = new KryptonTokens(inputStream); 
            var tokens = new CommonTokenStream(lexer); 
            var parser = new KryptonParser(tokens);

            var walker = new KryptonParseTreeWalker(name);
            var listener = new KryptonParserListener(this);

            try
            {
                walker.Walk(listener, parser.init());
            }
            // Rethrow any parser exceptions thrown while walking (avoids stack trace)
            catch (KryptonParserException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Populates the definition file with the builtin members
        /// </summary>
        /// <param name="container"></param>
        public void PopulateBuiltins()
        {
            AddMember(new BuiltinType("bool", this));
            AddMember(new BuiltinType("byte", this));
            AddMember(new BuiltinType("buffer", this));
            AddMember(new BuiltinType("int8", this));
            AddMember(new BuiltinType("uint8", this));
            AddMember(new BuiltinType("int16", this));
            AddMember(new BuiltinType("uint16", this));
            AddMember(new BuiltinType("int32", this));
            AddMember(new BuiltinType("uint32", this));
            AddMember(new BuiltinType("int64", this));
            AddMember(new BuiltinType("uint64", this));
            AddMember(new BuiltinType("string", this));
            AddMember(new BuiltinType("cstring", this));
            AddMember(new BuiltinType("array", this));
        }
        
        private interface IIncrementalFactory<out T>
        {
            int Current { get; }

            T Create(string name, IMemberContainer parent);
        }

        public class IncrementalMessageFactory : IIncrementalFactory<Message>
        {
            public int Current { get; private set; }

            public Message Create(string name, IMemberContainer parent)
            {
                Current += 1;
                return new Message(name, Current, parent);
            }
        }
        
        public class IncrementalGroupFactory : IIncrementalFactory<Group>
        {
            public int Current { get; private set; }

            public Group Create(string name, IMemberContainer parent)
            {
                Current += 1;
                return new Group(name, Current, parent);
            }
        }
    }
}
