using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
using System.Text;

namespace QShirt.ProxyGenerator;

[Generator]
public sealed class CommandsAndQueriesGenerator : ISourceGenerator
{
    #region Templates

    private const string ProxyTemplate = @"
using System;
using System.Collections.Generic;
using Telerik.DataSource;
using [AssemblyName].ServiceProxies;
using [AssemblyName].Generated.Dictionaries;

namespace [AssemblyName].Generated.[CommandsOrQueries].[EpicName];

public partial class [Name]: [BaseClass]
{
		public [Name](IExecutor executor) : base(executor) 
		{
		}
}

";

    private const string ProxiesDependenciesRegistrarTemplate = @"
using Microsoft.Extensions.DependencyInjection;
[Namespace];

namespace [AssemblyName].Generated.ServiceProxies
{
	public static class ProxiesDependencyRegistrar
	{
		public static IServiceCollection AddServiceProxies(this IServiceCollection services)
		{
			[Dependencies]
			
			return services;
		}
	}
}
";

    private const string DependencyTemplate = "services.AddScoped<[Name]>();";

    #endregion Templates

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

        StringBuilder dependeciesStringBuilder = new StringBuilder(); // for dependency registration

        foreach (var commandOrQuery in GetCommandsAndQueriesFromNamespace(applicationNamespace).Where(attr => !attr.GetAttributes().Any(att => att.AttributeClass.Name == nameof(NotProxyAttribute))))
        {
            bool isCommand = commandOrQuery.Name.EndsWith("Command");
            string proxyName = commandOrQuery.Name + "Proxy";

            // extract Execute method
            IMethodSymbol excuteMethod = commandOrQuery.GetMembers("Execute").SingleOrDefault() as IMethodSymbol;
            if (excuteMethod == null)
            {
                context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
                    "PG001",
                    "Execute method missing",
                    "Execute method is missing in {0}", "", DiagnosticSeverity.Error, true),
                    commandOrQuery.Locations.FirstOrDefault(), isCommand ? $"command {commandOrQuery.Name}" : $"query {commandOrQuery.Name}"));
                return;
            }

            // determine input and output model types
            INamedTypeSymbol inputType = excuteMethod.Parameters.FirstOrDefault()?.Type as INamedTypeSymbol;
            INamedTypeSymbol methodReturnType = excuteMethod.ReturnType as INamedTypeSymbol;
            INamedTypeSymbol resultType = null;
            if (methodReturnType.Name == nameof(Task) && methodReturnType.IsGenericType)
                resultType = methodReturnType.TypeArguments.First() as INamedTypeSymbol;
            else if (methodReturnType.Name != nameof(Task) && methodReturnType.Name != nameof(ValueTask) && methodReturnType.Name != typeof(void).Name)
                resultType = methodReturnType;

            // determine full proxy names for input and output models
            // [AssemblyName].Generated.Models.[EpicName].[Name]
            string inputModelFullName = null;
            string resultModelFullName = null;

            if (inputType != null)
            {
                if (inputType.Name == "DataSourceRequest") // for model from Telerik.DataSource
                    inputModelFullName = "DataSourceRequest";
                else if (inputType.Name == "IEnumerable" || inputType.Name == "ICollection")
                {

                    INamedTypeSymbol modelType = inputType.TypeArguments.FirstOrDefault() as INamedTypeSymbol;
                    if (modelType.ContainingNamespace.Name == "System")
                        inputModelFullName = $"{inputType.Name}<{modelType}>";
                    else
                        inputModelFullName = $"{inputType.Name}<{GetModelTypeFullName(modelType, assemblyName)}>";
                }

                else if (inputType.ContainingNamespace.Name == "System")
                    inputModelFullName = inputType.Name;
                else // for generated model
                    inputModelFullName = GetModelTypeFullName(inputType, assemblyName);

            }
            if (resultType != null)
            {
                if (resultType.Name == "DataResult" || resultType.Name == "IEnumerable" || resultType.Name == "ICollection") // for DataResult, IEnumerable, ICollection
                {
                    INamedTypeSymbol modelType = resultType.TypeArguments.FirstOrDefault() as INamedTypeSymbol;
                    resultModelFullName = $"{resultType.Name}<{GetModelTypeFullName(modelType, assemblyName)}>";
                }
                else // for generated models
                {
                    resultModelFullName = GetModelTypeFullName(resultType, assemblyName);
                }
            }

            // determine base class for proxy
            string baseClassName = string.Empty;
            if (inputType != null && resultType != null)
                baseClassName = $"ProxyBase<{inputModelFullName}, {resultModelFullName}>";
            else if (inputType == null && resultType != null)
                baseClassName = $"ProxyBaseNoInput<{resultModelFullName}>";
            else if (inputType != null && resultType == null)
                baseClassName = $"ProxyBaseNoResult<{inputModelFullName}>";
            else
                baseClassName = "ProxyBaseNoInputNoResult";

            string commandsOrQueries = isCommand ? "Commands" : "Queries";
            string epicName = commandOrQuery.ContainingNamespace.Name.Split('.').Last();
            string proxySource = ProxyTemplate
                .Replace("[AssemblyName]", assemblyName)
                .Replace("[CommandsOrQueries]", commandsOrQueries)
                .Replace("[EpicName]", epicName)
                .Replace("[BaseClass]", baseClassName)
                .Replace("[Name]", proxyName)
                //.Replace("[Microservice]", $"\"{microservice}\"")
                ;

            context.AddSource(proxyName, SourceText.From(proxySource, Encoding.UTF8));

            // add dependency
            string namespaceName = $"{assemblyName}.Generated.{commandsOrQueries}.{epicName}";
            string dependecy = DependencyTemplate.Replace("[Name]", $"{namespaceName}.{proxyName}");
            dependeciesStringBuilder.AppendLine(dependecy);
        }

        // dependency registrar
        string proxiesDependenciesRegistrar = ProxiesDependenciesRegistrarTemplate
            .Replace("[AssemblyName]", assemblyName)
            .Replace("[Namespace]", context.Compilation.AssemblyName.Contains("Backoffice") ? "using QShirt.Backoffice.Proxy.Generated" : "using QShirt.Public.Proxy.Generated")
            .Replace("[Dependencies]", dependeciesStringBuilder.ToString());
        context.AddSource("ProxiesDependencyRegistrar", SourceText.From(proxiesDependenciesRegistrar, Encoding.UTF8));
    }

    /// <summary>
    /// Returns all models from namespace
    /// </summary>
    private List<INamedTypeSymbol> GetCommandsAndQueriesFromNamespace(INamespaceSymbol @namespace)
    {
        var result = @namespace.GetTypeMembers().Where(t => t.Name.EndsWith("Command") || t.Name.EndsWith("Query")).ToList();

        var chilrenNamespaces = @namespace.GetNamespaceMembers();
        foreach (var childNamespace in chilrenNamespaces)
            result.AddRange(GetCommandsAndQueriesFromNamespace(childNamespace));

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
