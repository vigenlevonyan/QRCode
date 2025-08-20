using QShirt.Public.Grpc;
using System.Text.Json;

namespace QShirt.Public.Proxy.ServiceProxies;

/// <summary>
/// Base class for command and query proxies with input data and return value
/// </summary>
public abstract class ProxyBase<TInput, TResult>
{
    #region Fields

    private readonly IExecutor executor;

    #endregion Fields

    #region Constructor

    protected ProxyBase(IExecutor executor)
    {
        this.executor = executor;
    }

    #endregion Constructor

    /// <summary>
    /// Executes command/query
    /// </summary>
    /// <param name="input">Input data for command/query</param>
    /// <returns>Return result of command/query</returns>
    public Task<TResult> Execute(TInput input)
    {
        ExecuteInput executeInput = new ExecuteInput
        {
            CommandOrQueryName = GetType().Name.Replace("Proxy", string.Empty),
            Input = JsonSerializer.Serialize(input)
        };
        return executor.Execute<TResult>(executeInput);
    }
}

/// <summary>
/// Base class for command and query proxies without input data
/// </summary>
public abstract class ProxyBaseNoInput<TResult>
{
    #region Fields

    private readonly IExecutor executor;

    #endregion Fields

    #region Constructor

    protected ProxyBaseNoInput(IExecutor executor)
    {
        this.executor = executor;
    }

    #endregion Constructor

    /// <summary>
    /// Executes command/query
    /// </summary>
    /// <returns>Return result of command/query</returns>
    public Task<TResult> Execute()
    {
        ExecuteInput executeInput = new ExecuteInput
        {
            CommandOrQueryName = GetType().Name.Replace("Proxy", string.Empty)
        };
        return executor.Execute<TResult>(executeInput);
    }
}

/// <summary>
/// Base class for command and query proxies without return value
/// </summary>
public abstract class ProxyBaseNoResult<TInput>
{
    #region Fields

    private readonly IExecutor executor;

    #endregion Fields

    #region Constructor

    protected ProxyBaseNoResult(IExecutor executor)
    {
        this.executor = executor;
    }

    #endregion Constructor

    /// <summary>
    /// Executes command/query
    /// </summary>
    /// <param name="input">Input data for command/query</param>
    public Task Execute(TInput input)
    {
        ExecuteInput executeInput = new ExecuteInput
        {
            CommandOrQueryName = GetType().Name.Replace("Proxy", string.Empty),
            Input = JsonSerializer.Serialize(input)
        };
        return executor.Execute(executeInput);
    }
}

/// <summary>
/// Base class for command and query proxies without input data and return value
/// </summary>
public abstract class ProxyBaseNoInputNoResult
{
    #region Fields

    private readonly IExecutor executor;

    #endregion Fields

    #region Constructor

    protected ProxyBaseNoInputNoResult(IExecutor executor)
    {
        this.executor = executor;
    }

    #endregion Constructor

    /// <summary>
    /// Executes command/query
    /// </summary>
    public Task Execute()
    {
        ExecuteInput executeInput = new ExecuteInput
        {
            CommandOrQueryName = GetType().Name.Replace("Proxy", string.Empty)
        };
        return executor.Execute(executeInput);
    }
}