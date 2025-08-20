using Autofac;
using Grpc.Core;
using QShirt.Public.Application.Queries.Customers;
using System.Reflection;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace QShirt.Public.Server.Services;

public class ExecutorService : QShirt.Public.Grpc.Executor.ExecutorClient
{
    /// <summary>
    /// Information about command/query for their dynamic invocation
    /// </summary>
    private class CommandOrQueryInfo
    {
        public CommandOrQueryInfo(Type commandOrQueryType)
        {
            // through reflection we get all necessary information about commands/queries
            CommandOrQueryType = commandOrQueryType;
            ExecuteMethod = commandOrQueryType.GetMethods()
                .First(m => m.Name == "Execute");
            InputType = ExecuteMethod.GetParameters().FirstOrDefault()?.ParameterType;
            HasReturnValue = ExecuteMethod.ReturnType.IsGenericType;
            if (HasReturnValue)
            {
                ReturnValueType = ExecuteMethod.ReturnType.GenericTypeArguments.FirstOrDefault();

                // if not a model (or their collection) and not a simple type (or their collection) is returned,
                // then we consider that there is no return type
                var returnType = ReturnValueType.IsAssignableFrom(typeof(IEnumerable<>)) ?
                    ReturnValueType.GenericTypeArguments.First() :
                    ReturnValueType;

                if (returnType.Namespace != nameof(System) &&
                    !returnType.Name.EndsWith("Model") &&
                    !returnType.Name.StartsWith("DataResult")
                    && !returnType.Name.StartsWith("IEnumerable")
                    )
                {
                    HasReturnValue = false;
                    ReturnValueType = null;
                }
            }
        }

        /// <summary>
        /// Type (System.Type) of command/query
        /// </summary>
        public Type CommandOrQueryType { get; }

        /// <summary>
        /// Information about Execute method of command/query
        /// </summary>
        public MethodInfo ExecuteMethod { get; }

        /// <summary>
        /// Type (System.Type) of input argument
        /// </summary>
        public Type InputType { get; }

        /// <summary>
        /// Type (System.Type) of return value
        /// </summary>
        public Type ReturnValueType { get; }

        /// <summary>
        /// Is there a return value?
        /// </summary>
        public bool HasReturnValue { get; }
    }

    #region Fields

    private readonly ILifetimeScope lifetimeScope;


    private static readonly Dictionary<string, CommandOrQueryInfo> CommandsAndQueries =
        new Dictionary<string, CommandOrQueryInfo>();

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initializes ExecutorService, collecting information about all commands/queries in the subsystem
    /// </summary>
    public static void Init()
    {
        var commandsAndQueriesTypes =
            Assembly
                .GetAssembly(typeof(GetCustomerContentQuery))
                .GetTypes()
                .Where(t => t.Name.EndsWith("Query") || t.Name.EndsWith("Command"));

        foreach (var commandOrQuery in commandsAndQueriesTypes)
        {
            CommandOrQueryInfo commandOrQueryInfo = new CommandOrQueryInfo(commandOrQuery);
            CommandsAndQueries.Add(commandOrQuery.Name, commandOrQueryInfo);
        }
    }

    public ExecutorService(ILifetimeScope lifetimeScope)
    {
        this.lifetimeScope = lifetimeScope;
    }

    #endregion Constructors

    public override async Task<QShirt.Public.Grpc.ExecuteResult> ExecuteAsync(QShirt.Public.Grpc.ExecuteInput request, ServerCallContext context)
    {
        // extract information about command or query
        CommandOrQueryInfo commandOrQueryInfo = CommandsAndQueries[request.CommandOrQueryName];

        // get command or query from DI container
        var command = lifetimeScope.Resolve(commandOrQueryInfo.CommandOrQueryType!);

        List<object> inputParams = new List<object>();
        if (!string.IsNullOrEmpty(request.Input))
        {
            // deserialize input data
            var input = JsonSerializer.Deserialize(request.Input, commandOrQueryInfo.InputType);
            inputParams.Add(input);
        }

        // get Task executing command or query; wait for its completion
        Task executionTask = commandOrQueryInfo.ExecuteMethod
            .Invoke(command, inputParams.ToArray()) as Task;
        await executionTask;

        // get result of command/query execution
        if (commandOrQueryInfo.HasReturnValue)
        {
            var resultProperty = typeof(Task<>).MakeGenericType(commandOrQueryInfo.ReturnValueType)
                .GetProperty("Result");
            var result = resultProperty.GetValue(executionTask);

            // return serialized result
            return new QShirt.Public.Grpc.ExecuteResult
            {
                Result = JsonSerializer.Serialize(result)
            };
        }

        return new QShirt.Public.Grpc.ExecuteResult();
    }
}