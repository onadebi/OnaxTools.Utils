using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace OnaxTools.Services.DbAccess
{
    public interface IMongoDbAccess
    {
        IMongoCollection<T> GetCollection<T>();
        Task<string> CreateUniqueIndex<T>(IMongoCollection<T> collection, string fieldName, [CallerMemberName] string callerName = "");
        Task<List<T>> GetData<T>(Expression<Func<T, bool>> predicate = null, [CallerMemberName] string callerName = "");
        Task<T> InsertRecord<T>(T entry, [CallerMemberName] string callerName = "");
    }

    public class MongoDbAccess : IMongoDbAccess
    {
        private readonly ILogger<MongoDbAccess> _logger;
        private readonly IMongoDatabase _db;
        public MongoDbAccess(string conString, string database, ILogger<MongoDbAccess> logger)
        {
            _logger = logger;
            _db = new MongoClient(conString).GetDatabase(database);
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            var col = _db.GetCollection<T>(typeof(T).Name);
            return col;
        }

        public async Task<string> CreateUniqueIndex<T>(IMongoCollection<T> collection, string indexName, [CallerMemberName] string callerName = "")
        {
            string indexNameResult = string.Empty;
            try
            {
                List<AppBsonIndexes> indexes = new List<AppBsonIndexes>();
                var allIndexes = await (await collection.Indexes.ListAsync()).ToListAsync();
                for (int i = 0; i < allIndexes.Count; i++)
                {
                    var index = allIndexes[i];
                    var itemDictionary = index.ToDictionary();
                    string indexString = System.Text.Json.JsonSerializer.Serialize(itemDictionary);
                    var desItem = System.Text.Json.JsonSerializer.Deserialize<AppBsonIndexes>(indexString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    indexes.Add(desItem);
                };

                // If index not present create it else skip.
                if (indexes.Where(i => i.Name.Equals(indexName, StringComparison.CurrentCultureIgnoreCase)).Any() == false)
                {
                    // Create Index here
                    indexName = collection.Indexes.CreateOne(
                    new CreateIndexModel<T>(Builders<T>.IndexKeys.Ascending(m => indexName),
                    new CreateIndexOptions { Unique = true })
                    );
                }
                else
                {
                    indexNameResult = $"Index name ${indexName} already exists";
                }
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, $"[{callerName}] ::> {ex.Message}");
            }
            return indexNameResult;
        }

        public async Task<List<T>> GetData<T>(Expression<Func<T, bool>> predicate = null, [CallerMemberName] string callerName = "")
        {
            List<T> objResp = new();
            try
            {
                if (predicate != null)
                {
                    objResp = await _db.GetCollection<T>(typeof(T).Name).Find(predicate).ToListAsync();

                }
                else
                {
                    objResp = await _db.GetCollection<T>(typeof(T).Name).Find(_ => true).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[{callerName}] ::> {ex.Message}");
            }
            return objResp;
        }

        public async Task<T> InsertRecord<T>(T entry, [CallerMemberName] string callerName = "")
        {
            try
            {
                await _db.GetCollection<T>(typeof(T).Name).InsertOneAsync(entry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[{callerName}] ::> {ex.Message}");
            }
            return entry;
        }


    }


    #region Fillers
    public class AppBsonIndexes
    {
        public int V { get; set; }
        public bool Unique { get; set; }
        //public int key { get; set; }
        public string Name { get; set; }
        public string Ns { get; set; }
    }
    #endregion
}
