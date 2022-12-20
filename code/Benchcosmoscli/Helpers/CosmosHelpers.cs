using Azure.Identity;
using Benchcosmoscli.Benchmarks;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchcosmoscli.Helpers
{
    public class CosmosHelpers
    {
        public static Task<ItemResponse<T>> InsertItem<T>(string databaseName, string containerName, T Element)
        {
            using CosmosClient client = new(
                   accountEndpoint: Environment.GetEnvironmentVariable("COSMOS_ENDPOINT")!,
                   authKeyOrResourceToken: Environment.GetEnvironmentVariable("COSMOS_KEY")!
            );
            var db = client.GetDatabase(databaseName);
            var container = db.GetContainer(containerName);
            return container.CreateItemAsync<T>(Element);

        }

        public static async Task<TransactionalBatchOperationResult<T>> InsertTransacctionalBatch<T>(string databaseName, string containerName, string partitionKeyName, List<T> Elements)
        {
            using CosmosClient client = new(
                   accountEndpoint: Environment.GetEnvironmentVariable("COSMOS_ENDPOINT")!,
                   authKeyOrResourceToken: Environment.GetEnvironmentVariable("COSMOS_KEY")!
            );
            var db = client.GetDatabase(databaseName);
            var container = db.GetContainer(containerName);
            PartitionKey partitionKey = new PartitionKey(partitionKeyName);
            TransactionalBatch batch = container.CreateTransactionalBatch(partitionKey);
            foreach (var item in Elements)
                batch.CreateItem<T>(item);
            using TransactionalBatchResponse response = await batch.ExecuteAsync();
            if (response.IsSuccessStatusCode)
            {
               TransactionalBatchOperationResult<T> productResponse;
               return response.GetOperationResultAtIndex<T>(0);
            }
            return null;
        }

        public static async Task<CosmosTestRun<T>> QueryItems<T>(string databaseName, string containerName, QueryDefinition  query)
        {
           List<T> collection = new List<T>();
           double consumedRUs = 0;
            using CosmosClient client = new(
                   accountEndpoint: Environment.GetEnvironmentVariable("COSMOS_ENDPOINT")!,
                   authKeyOrResourceToken: Environment.GetEnvironmentVariable("COSMOS_KEY")!
            );
            var db = client.GetDatabase(databaseName);
            var container = db.GetContainer(containerName);
            using FeedIterator<T> feed = container.GetItemQueryIterator<T>(query);

            while (feed.HasMoreResults)
            {
                FeedResponse<T> response = await feed.ReadNextAsync();
                consumedRUs += response.RequestCharge;
                foreach (T item in response)
                {
                    collection.Add(item);
                }
            }
            return CosmosTestRun<T>.SaveInTestRun(collection, consumedRUs);
        }

    }
}
