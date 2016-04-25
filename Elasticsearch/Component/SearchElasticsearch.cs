using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.ElasticSearch.Interface;
using Microbrewit.Api.Settings;
using Microsoft.Extensions.OptionsModel;
using Nest;

namespace Microbrewit.Api.ElasticSearch.Component
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

        public async Task<IEnumerable<dynamic>> SearchIngredientsAsync(string query, int @from, int size)
        {
            var ingredients = new[]{"hop","other","fermentable","yeast"};
            var boolQuery = Query<dynamic>.Bool(b => b.Filter(f => f.Terms(t => t.Field("type").Terms(ingredients))).Must(m => m.Match(ma => ma.Field("name").Query(query))));
            var res = await _client.SearchAsync<dynamic>(new SearchRequest()
            {
                From = from,
                Size = size,
                Query = boolQuery
            });
            return res.Documents;
        }
    }
}
