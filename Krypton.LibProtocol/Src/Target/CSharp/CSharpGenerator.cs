﻿using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Antlr4.StringTemplate;
using Krypton.LibProtocol.Extensions;
using Krypton.LibProtocol.File;
using Krypton.LibProtocol.Member;
using Krypton.LibProtocol.Member.Common;
using Krypton.LibProtocol.Member.Declared;
using Krypton.LibProtocol.Member.Type;

namespace Krypton.LibProtocol.Target.CSharp
{
    public class CSharpGenerator : LanguageGenerator<CSharpTargetSettings>
    {
        protected override string TemplatesPath => "Krypton/LibProtocol/Templates/";
        
        public CSharpGenerator(DefinitionFile file) : base(file)
        {
        }

        public override void Generate(CSharpTargetSettings settings)
        {
            var template = ReadTemplate("csharp.stg");
            RegisterModelAdaptors(template);
            
            // each root member gets its own file
            foreach (var member in File.Members)
            {
                // skip builtin types
                if (member is BuiltinType)
                {
                    continue;
                }

                var targetdecl = template.GetInstanceOf("render_member");
                targetdecl.Add("member", member);
                var render = targetdecl.Render();
                
                var path = Path.Combine(settings.OutDirectory, $"{member.Name}.cs");
                System.IO.File.WriteAllText(path, render);
            } 
        }

        private static void RegisterModelAdaptors(TemplateGroup template)
        {
            template.RegisterRenderer(typeof(Documentation), new DocumentationRenderer());
            template.RegisterModelAdaptor(typeof(ITypeReference), new ITypeReferenceAdaptor());
            template.RegisterModelAdaptor(typeof(Packet), new PacketReferenceAdaptor());
            
            // register base model adaptors as fallbacks
            template.RegisterModelAdaptor(typeof(INameable), new INameableModelAdaptor());
        }

        private class DocumentationRenderer : IAttributeRenderer
        {
            public string ToString(object obj, string formatString, CultureInfo culture)
            {
                var documentation = obj as Documentation;
                return documentation.Text;
            }
        }

        private class PacketReferenceAdaptor : TargetModelAdaptor
        {
            [Model("classpath")]
            public string Path(Packet packet)
            {
                var namespaces = new List<string>();

                var currentMem = packet.Parent as IMember;
                while (currentMem != null)
                {
                    namespaces.Add(currentMem.Name.ToCamelCase());
                    currentMem = currentMem.Parent as IMember;
                }
                
                namespaces.Reverse();
                namespaces.Add(packet.Name.ToCamelCase());
                return string.Join(".", namespaces);
            }
        }

        private class ITypeReferenceAdaptor : TargetModelAdaptor
        {
            [Model("namespace")]
            public string Namespace(ITypeReference typeref)
            {
                if (typeref.Scope is DefinitionFile)
                {
                    return "Krypton.LibProtocol";
                }

                var namespaces = new List<string>();

                var currentMem = typeref.Scope as IMember;
                while (currentMem != null)
                {
                    namespaces.Add(currentMem.Name.ToCamelCase());
                    currentMem = currentMem.Parent as IMember;
                }

                if (namespaces.Count == 0)
                {
                    return null;
                }

                namespaces.Reverse();
                return string.Join(".", namespaces);
            }
        }

        // base adaptors
        
        private class INameableModelAdaptor : TargetModelAdaptor
        {
            [Model("name")]
            public string Name(INameable nameable)
            {
                return nameable.Name.ToCamelCase();
            }
        }
    }
}