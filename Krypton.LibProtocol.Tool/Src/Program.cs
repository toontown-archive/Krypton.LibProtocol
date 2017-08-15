using System;
using Microsoft.Extensions.CommandLineUtils;
using Krypton.LibProtocol.File;
using Krypton.LibProtocol.File.Util;
using Krypton.LibProtocol.Target.CSharp;

namespace Krypton.LibProtocol.Tool
{
    public class Program
    {   
        public static void Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.HelpOption("-h | --help");
                
            // Target output language.
            var language = app.Option(
                "--language <language>", 
                "The language of the generated files.", 
                CommandOptionType.SingleValue);
    
            // Target output directory.
            var output = app.Option(
                "-o <directory>", 
                "Output directory of the generated files.", 
                CommandOptionType.SingleValue);
            
            // Include directories.
            var includes = app.Option(
                "-I <directory>", 
                "Include directory.", 
                CommandOptionType.MultipleValue);

            // Files to read in
            var files = app.Argument(
                "[filename]",
                "Files to read",
                multipleValues: true);

            app.OnExecute(() => ExecuteApp(language, output, includes, files));
            app.Execute(args);
        }

        private static int ExecuteApp(CommandOption language, CommandOption output, CommandOption includes, CommandArgument files)
        {
            // Currently we only support CSharp...
            if (language.Value() != "CSharp")
            {
                throw new NotSupportedException("Unsupported language: " + language.Value());
            }

            // Create a new file resolver and include the passed directories
            var resolver = new ContextualFileResolver();
            foreach (var include in includes.Values)
            {
                resolver.Include(include);
            }

            // Load in the passed files
            var pf = new DefinitionFile
            {
                Resolver = resolver
            };
            pf.PopulateBuiltins();

            foreach (var file in files.Values)
            {
                pf.Load(file);
            }

            // Generate. TODO: write in a way that easily allows for using different target languages         
            var settings = new CSharpTargetSettings
            {
                OutDirectory = output.Value()
            };
            var generator = new CSharpGenerator(pf);
            generator.Generate(settings);
            return 0;
        }
    }
}
