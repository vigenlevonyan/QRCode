using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Nc.Domain.BuiltInDictionaries;
using System.Diagnostics;
using System.Text;

namespace QShirt.ProxyGenerator
{
    [Generator]
    public class DictionariesGenerator : ISourceGenerator
    {
        private const string EnumTemplate = @"
using Nc.Domain.BuiltInDictionaries;


namespace [AssemblyName].Generated.Dictionaries;

public enum [Name]
{
[Members]
}
";

        private const string MemberTemplate = @"
/// <summary>
/// [RusName]
/// </summary>
[Name(""[RusName]"")]
[Name] = [Value],
";


        public void Execute(GeneratorExecutionContext context)
        {
            string assemblyName = context.Compilation.AssemblyName;

            int index = 0;// context.Compilation.AssemblyName.Contains("Public") ? 17 : 0;
            // Get Domain project namespace
            //INamespaceSymbol domainNamespace = context.Compilation.GlobalNamespace
            //    .GetNamespaceMembers().ToList()[0]
            //    .GetMembers().ToList()[0]
            //    .GetMembers().Single(n => n.Name.Contains("Domain"))
            //    as INamespaceSymbol;

            INamespaceSymbol domainNamespace = context.Compilation.GlobalNamespace
            .GetNamespaceMembers().ToList()[index]
            //.GetMembers().ToList()[0]
            //.GetMembers().Where(n => n.Name.Contains("QShirt")).Cast<INamespaceSymbol>().Single()
            .GetMembers().Single(n => n.Name.Contains("Domain"))
            as INamespaceSymbol;

            // for each enum in namespace generate dictionary code
            foreach (INamedTypeSymbol type in GetEnumsFromNamespace(domainNamespace))
            {
                // Create dictionary values code
                StringBuilder membersBuilder = new StringBuilder();
                foreach (IFieldSymbol member in type.GetMembers().Where(m => m is IFieldSymbol))
                {
                    string rusName = member.GetAttributes().Single(a => a.AttributeClass.Name == nameof(NameAttribute)).ConstructorArguments[0].Value.ToString();
                    string memberSource = MemberTemplate
                        .Replace("[RusName]", rusName)
                        .Replace("[Name]", member.Name)
                        .Replace("[Value]", member.ConstantValue.ToString());
                    membersBuilder.AppendLine(memberSource);
                }

                // Create dictionary code
                string source = EnumTemplate
                    .Replace("[AssemblyName]", assemblyName)
                    .Replace("[Name]", type.Name)
                    .Replace("[Members]", membersBuilder.ToString());

                context.AddSource(type.Name, SourceText.From(source, Encoding.UTF8));
            }
        }

        /// <summary>
        /// Returns all enums from namespace
        /// </summary>
        private List<INamedTypeSymbol> GetEnumsFromNamespace(INamespaceSymbol @namespace)
        {
            var result = @namespace.GetTypeMembers().Where(t => t.TypeKind == TypeKind.Enum).ToList();

            var childNamespaces = @namespace.GetNamespaceMembers();
            foreach (var childNamespace in childNamespaces)
                result.AddRange(GetEnumsFromNamespace(childNamespace));

            return result;
        }

        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            if (!Debugger.IsAttached)
            {
                // Debugger.Launch();
            }
#endif
        }
    }
}
