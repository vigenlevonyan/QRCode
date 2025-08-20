using Grpc.Core;
using System.Text.Json;
using QShirt.Public.Grpc;

namespace QShirt.Public.Client
{
    public class Executor : Proxy.ServiceProxies.IExecutor
    {
        private readonly QShirt.Public.Grpc.Executor.ExecutorClient executorClient;
        private readonly UserSession userSession;

        internal Executor(QShirt.Public.Grpc.Executor.ExecutorClient executorClient, UserSession userSession)
        {
            this.executorClient = executorClient;
            this.userSession = userSession;
        }

        public async Task<TResult> Execute<TResult>(ExecuteInput input)
        {
            Metadata headers = new Metadata();
            if (userSession.Token != null)
                headers.Add("Authorization", $"Bearer {userSession.Token}");
            var executionResult = await executorClient.ExecuteAsync(input, headers);

            return JsonSerializer.Deserialize<TResult>(executionResult.Result);
        }

        public async Task Execute(ExecuteInput input)
        {
            Metadata headers = new Metadata();
            if (userSession.Token != null)
                headers.Add("Authorization", $"Bearer {userSession.Token}");
            await executorClient.ExecuteAsync(input, headers);
        }
    }
}