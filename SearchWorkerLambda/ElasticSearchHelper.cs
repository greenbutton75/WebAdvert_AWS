using System;
using Amazon.Runtime;
using Elasticsearch.Net;
using Elasticsearch.Net.Aws;
using Microsoft.Extensions.Configuration;
using Nest;

namespace SearchWorkerLambda
{
    public static class ElasticSearchHelper
    {
        private static ElasticClient _client;

        public static ElasticClient GetInstance(IConfiguration config)
        {
            if (_client == null)
            {
                var httpConnection = new AwsHttpConnection();
                var url = config.GetSection("ES").GetValue<string>("url");
                var pool = new SingleNodeConnectionPool(new Uri(url));
                var elconfig = new ConnectionSettings(pool, httpConnection)
                            .DefaultIndex("adverts")
                    .DefaultMappingFor<AdvertType>(m => m.IdProperty(x => x.Id)); ;


                _client = new ElasticClient(elconfig);

            }

            return _client;
        }
    }
}