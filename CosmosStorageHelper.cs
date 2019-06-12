using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace TestFunction
{
    /// <summary>
    /// Class which has cosmos storage accesor api's
    /// </summary>
    public class CosmosStorageHelper
    {
        /// <summary>
        /// Retrieves from store
        /// </summary>
        /// <param name="table"></param>
        /// <param name="tableQuery"></param>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public static async Task<List<T>> RetrieveFromStorageAsync<T>(CloudTable table, TableQuery<T> tableQuery, string requestId) where T : ITableEntity, new()
        {
            List<T> entities = new List<T>();

            TableContinuationToken continuationToken = null;
            do
            {
                // Retrieve a segment (up to 1,000 entities). 
                var tableQueryResult = await table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);

                continuationToken = tableQueryResult.ContinuationToken;
                entities.AddRange(tableQueryResult.Results);

            } while (continuationToken != null);

            return entities;
        }
    }
}
