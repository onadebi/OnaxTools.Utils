using Microsoft.Azure.Cosmos;
using System.Runtime.CompilerServices;

namespace OnaxTools.Services.DbAccess
{
    public interface ICosmosDbAccess
    {
        //List<T> GetCollection<T>();
        //Task<List<T>> GetData<T>(Expression<Func<T, bool>> predicate = null, [CallerMemberName] string callerName = "");
        Task<T> InsertRecord<T>(T entry, string partitionKey, [CallerMemberName] string callerName = "");
    }

    public class CosmosDbAccess : ICosmosDbAccess
    {
        private readonly Microsoft.Azure.Cosmos.Container _container;

        public CosmosDbAccess(CosmosClient dbClient,
                string databaseName,
                string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task<T> InsertRecord<T>(T entry, string partitionKey, [CallerMemberName] string callerName = "")
        {
            ItemResponse<T> objResp = await _container.UpsertItemAsync<T>(entry, partitionKey: new PartitionKey(partitionKey));
            return objResp;
        }
    }
}
