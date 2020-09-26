using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class CosmosDBTriggerCSharp1
    {
        [FunctionName("CosmosDBTriggerCSharp1")]

        // Essa POC eu pego um evento que ocorrou no CosmosDb e logo na tela
        public static void Run([CosmosDBTrigger(
            databaseName: "ToDoList",
            collectionName: "Items",
            ConnectionStringSetting = "AzureWebJobsStorage",
            LeaseCollectionName = "leases")]IReadOnlyList<Document> input, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);

                //Aqui eu pego o registro no Banco de Dados                
                var registro = input[0];
                //Aqui eu pego uma propriedade da tabela alterada no banco de dados.
                log.LogInformation(registro.GetPropertyValue<String>("nome"));

                //Tenho que lembrar de atualizar o local.settings no portal azure **Ele não é deployado na publicação**

            }
        }
    }
}
