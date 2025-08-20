using QShirt.Public.Grpc;
namespace QShirt.Public.Proxy.ServiceProxies;

/// <summary>
/// Interface for executor
/// </summary>
public interface IExecutor
{
    /// <summary>
    /// Executes specified command/query with specified parameters
    /// </summary>
    /// <typeparam name="TResult">Result type</typeparam>
    /// <param name="input">Input information</param>       
    Task<TResult> Execute<TResult>(ExecuteInput input);

    /// <summary>
    /// Executes specified command/query with specified parameters
    /// </summary>
    /// <param name="input">Input information</param>
    Task Execute(ExecuteInput input);
}
