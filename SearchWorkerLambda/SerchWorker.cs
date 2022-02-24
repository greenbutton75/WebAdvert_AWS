using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AdvertApi.Models.Messages;
using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Nest;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace SearchWorkerLambda
{
    public class Function
    {
        public Function() : this(ElasticSearchHelper.GetInstance(ConfigurationHelper.Instance))
        {

        }

        private readonly ElasticClient _client;
        public Function(ElasticClient client)
        {
            _client = client;
        }
        public async Task FunctionHandler(SNSEvent snsEvent, ILambdaContext context)
        {
            foreach (var record in snsEvent.Records)
            {
                context.Logger.LogLine(record.Sns.Message);

                var message = JsonSerializer.Deserialize<AdvertConfirmedMessage>(record.Sns.Message);
                var advertDocument = MappingHelper.Map(message);
                
                var ir = await _client.IndexDocumentAsync(advertDocument);
                context.Logger.LogLine(ir.DebugInformation);
            }
        }
    }
}

