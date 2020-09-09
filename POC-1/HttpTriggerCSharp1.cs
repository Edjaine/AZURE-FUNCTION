using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace Company.Function
{
    public static class HttpTriggerCSharp1
    {
        [FunctionName("HttpTriggerCSharp1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;


            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            int rowAfect = await CriaRegistro(responseMessage);

            return new OkObjectResult($" Registros afetados {rowAfect} ");
        }
        public static async Task<int> CriaRegistro(string mensagem)
        {
            var str = "Server=tcp:sql-server-db.database.windows.net,1433;Initial Catalog=fn5;Persist Security Info=False;User ID=edjaine;Password=asdqwe123!!!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                var text = $"INSERT INTO LOG (MENSAGEM) VALUES ('----> {mensagem}')";

                using (SqlCommand cmd = new SqlCommand(text, conn))
                {                    
                    return await cmd.ExecuteNonQueryAsync();                    
                }
            }
        }
    }
}
