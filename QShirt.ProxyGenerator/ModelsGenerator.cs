using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
using System.Text;

namespace QShirt.ProxyGenerator;
[Generator]
public class ModelsGenerator : ISourceGenerator
{
    private const string ModelTemplate = @"
using System;
using System.Collections.Generic;
using Telerik.DataSource; 
using Nc.AuditLogging.Abstractions;
using [AssemblyName].Generated.Dictionaries;
using [AssemblyName].Generated.Models;
namespace [AssemblyName].Generated.Models.[EpicName];

public class [Name]
{
[Properties]
}
";
    private const string PropertyTemplate = @"public [Type] [Name] { get; set; }";

    public void Execute(GeneratorExecutionContext context)
    {
        string assemblyName = context.Compilation.AssemblyName;

        int index = 0;// context.Compilation.AssemblyName.Contains("Public") ? 17 : 0;

        // Get Application project namespace
        INamespaceSymbol applicationNamespace = context.Compilation.GlobalNamespace
            .GetNamespaceMembers().ToList()[index]
            //.GetMembers().ToList()[0]
            .GetMembers().Where(n => n.Name.Contains("Backoffice") || n.Name.Contains("Public")).Cast<INamespaceSymbol>().Single()
            .GetMembers().Single(n => n.Name.Contains("Application"))
            as INamespaceSymbol;

        foreach (var model in GetModelsFromNamespace(applicationNamespace))
        {
            // create properties code
            StringBuilder propertiesSourceBuilder = new StringBuilder();
            string inputModelFullName = null;
            foreach (var property in model.GetMembers().Where(m => m.Kind == SymbolKind.Property && m.DeclaredAccessibility == Accessibility.Public).Cast<IPropertySymbol>())
            {
                string propertySource = string.Empty;
                INamedTypeSymbol propertyType = null;
                if (property.Type.ToString() == "byte[]")
                    propertySource = PropertyTemplate.Replace("[Type]", "byte[]");
                else
                    propertyType = property.Type as INamedTypeSymbol;

                if (propertyType != null && propertyType.IsGenericType)
                {
                    INamedTypeSymbol modelType = propertyType.TypeArguments.FirstOrDefault() as INamedTypeSymbol;
                    if (modelType.ContainingNamespace.Name == "System" || modelType.ContainingNamespace.Name == "Dictionaries")
                        inputModelFullName = $"{propertyType.Name}<{modelType}>";
                    else
                        inputModelFullName = $"{propertyType.Name}<{GetModelTypeFullName(modelType, assemblyName)}>";

                    propertySource = PropertyTemplate.Replace("[Type]", inputModelFullName);
                }
                else if (propertyType != null)
                    propertySource = PropertyTemplate.Replace("[Type]", property.Type.Name);

                propertiesSourceBuilder.AppendLine(propertySource.Replace("[Name]", property.Name));
            }

            string modelSource = ModelTemplate
                .Replace("[AssemblyName]", assemblyName)
                .Replace("[EpicName]", model.ContainingNamespace.ContainingNamespace.Name)
                .Replace("[Name]", model.Name)
                .Replace("[Properties]", propertiesSourceBuilder.ToString());

            context.AddSource(model.Name, SourceText.From(modelSource, Encoding.UTF8));
        }
    }

    /// <summary>
    /// Returns all models from namespace
    /// </summary>
    private List<INamedTypeSymbol> GetModelsFromNamespace(INamespaceSymbol @namespace)
    {
        var result = @namespace.GetTypeMembers().Where(t => t.Name.EndsWith("Model")).ToList();

        var childrenNamespaces = @namespace.GetNamespaceMembers();
        foreach (var childNamespace in childrenNamespaces)
            result.AddRange(GetModelsFromNamespace(childNamespace));

        return result;
    }

    /// <summary>
    /// Returns full name (including namespace) of generated model based on model from Application
    /// </summary>
    /// <param name="modelType">Model type from Application</param>
    private string GetModelTypeFullName(INamedTypeSymbol modelType, string assemblyName)
    {
        if (modelType == null)
            return null;

        string epicName = modelType?.ContainingNamespace.ContainingNamespace.Name;
        return $"{assemblyName}.Generated.Models.{epicName}.{modelType.Name}";
    }


    public void Initialize(GeneratorInitializationContext context)
    {
#if DEBUG
        if (!Debugger.IsAttached)
        {
            //Debugger.Launch();
        }
#endif
    }
}

