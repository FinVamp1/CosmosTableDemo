using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.Cosmos.Table;
using System.Configuration;

namespace TestFunction
{
    

    public static class Function1
    {
        static Function1()
        {
            try
            {
                ApplicationHelper.Startup();
            }
            catch(Exception e)
            {

            }
        }

        private static CloudStorageAccount storageAccount;
        private static CloudTableClient storageTableClient;

        [FunctionName("Function1")]
        public static void Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)]TimerInfo myTimer, TraceWriter log)
        {
            storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["tableConnStr"]);
            storageTableClient = storageAccount.CreateCloudTableClient();
            storageTableClient.GetTableReference("stamps");
            Util.RetreiveAsync(Guid.NewGuid().ToString(), storageTableClient.GetTableReference("NAM_DistributionMessage"), "1", "2");
        }
    }
}
