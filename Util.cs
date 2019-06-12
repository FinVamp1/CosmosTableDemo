using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace TestFunction
{
    public static class Util
    {
         /// <summary>
        /// retrieve message based on partiition key and row key.
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="tableName"></param>
        /// <param name="paritionKey"></param>
        /// <param name="rowkey"></param>
        /// <returns></returns>
        public static DistributeTableEntity RetreiveAsync(string requestId, CloudTable tableName, string paritionKey, string rowkey)
        {
            List<DistributionMessage> requests;
            try
            {
                TableQuery<DistributionMessage> tableQuery = new TableQuery<DistributionMessage>().Where(TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.LessThanOrEqual, DateTimeOffset.UtcNow.AddMinutes(-1)));
                tableQuery.TakeCount = 100;

                requests = CosmosStorageHelper.RetrieveFromStorageAsync(tableName, tableQuery, requestId).Result;
            }
            catch (Exception ex)
            {
            throw ex;
            }

            return requests.Count > 0 ? requests[0] : null;
            }

        }

    /// <summary>
    /// Distribution Table entiy ( base entity for distribution service table entities)
    /// </summary>
    public class DistributeTableEntity : TableEntity
    {
        public string RequestId;
        public string TenantId;
        public DateTime? CreatedOn;
    }

    /// <summary>
    /// Table entity to store distribute messages
    /// </summary>
    public class DistributionMessage : DistributeTableEntity
    {
        public DistributionMessage()
        { }

        /// <summary>
        /// Stores the status of request
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Stores the region of where the request is being served from
        /// </summary>
        public bool? IsPrimaryregion { get; set; }

        /// <summary>
        /// stores storage id for blob
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? ReplayCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? NumerOfBatches { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? isDLExpansionSent { get; set; }
    }

    /// <summary>
    /// Enum to hold distribution content table names
    /// </summary>
    public enum ContentTableNames
    {
        DistributionMessage,
        DistributionBatchMessage
    }
}
