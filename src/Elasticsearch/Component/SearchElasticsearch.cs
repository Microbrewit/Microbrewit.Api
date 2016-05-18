using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Elasticsearch.Interface;
using Microbrewit.Api.Settings;
using Microsoft.Extensions.Options;
using Nest;

namespace Microbrewit.Api.Elasticsearch.Component
{
    public class SearchElasticsearch : ISearchElasticsearch
    {
        private readonly ElasticSearchSettings _elasticSearchSettings;
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;

        public SearchElasticsearch(IOptions<ElasticSearchSettings> elasticsearchSettings)
        {
            _elasticSearchSettings = elasticsearchSettings.Value;
            this._node = new Uri(_elasticSearchSettings.Url);
            this._settings = new ConnectionSettings(_node);
            this._client = new ElasticClient(_settings);
        }


        public async Task<IEnumerable<dynamic>> SearchAllAsync(string query, int @from, int size)
        {
            
            var queryString = "{\"from\" : " + from + ", \"size\" : " + size + ", \"query\":{\"match\": {\"name\": {\"query\": \" " + query + " \",\"operator\": \"and\"}}}}";
            //            _client.SearchAsync<string>();
            //var res = await _client.SearchAsync<string>(s => s.Query(q => q.QueryString(qs => qs.Query(queryString))));
            var res = await _client.SearchAsync<dynamic>(s => s.Query(q => q.Match(m => m.Field("name").Query(query))));
            return res.Documents;
            //return Task.FromResult("");
        }

     
    }
}
