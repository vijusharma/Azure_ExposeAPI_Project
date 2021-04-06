using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace function_ExposeAPI
{
    public static class Fun_CreateAPI
    {
        [FunctionName("CreateAPI")]
        public static async Task Run(
          [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestMessage req,
          [Table("StorageTable")] ICollector outTable,
          TraceWriter log)
        {
            dynamic data = await req.Content.ReadAsAsync<object>();
            string ExposeAPI = data?.ExposeAPI;

            if (ExposeAPI == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass text in the request body");
            }

            outTable.Add(new API()
            {
                PartitionKey = "APIData",
                RowKey = Guid.NewGuid().ToString(),
                exposeAPI = ExposeAPI
            });
            return req.CreateResponse(HttpStatusCode.Created);
        }

        public class API : TableEntity
        {
            public string exposeAPI { get; set; }
        }
    }
}