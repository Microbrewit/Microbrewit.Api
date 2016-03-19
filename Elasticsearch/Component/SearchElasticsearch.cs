using System;
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
        private int _bigNumber = 10000;

        public SearchElasticsearch(IOptions<ElasticSearchSettings> elasticsearchSettings)
        {
            _elasticSearchSettings = elasticsearchSettings.Value;
            this._node = new Uri(_elasticSearchSettings.Url);
            this._settings = new ConnectionSettings(_node);
            this._client = new ElasticClient(_settings);
        }


        public async Task<string> SearchAllAsync(string query, int @from, int size)
        {
            // var queryString = "{\"from\" : " + from + ", \"size\" : " + size + ", \"query\":{\"match\": {\"name\": {\"query\": \" " + query + " \",\"operator\": \"and\"}}}}";
            //             _client.SearchAsync<string>();
            // var res = await _client.SearchAsync<string>( _elasticSearchSettings.Index,queryString);
            // return res.Response;
            return "";
        }

        public async Task<string> SearchIngredientsAsync(string query, int @from, int size)
        {
            // var queryString = "{\"from\": " + from +", \"size\": " + size +", \"filter\": { \"or\": [{\"term\": { \"dataType\": \"hop\"}},{\"term\": {\"dataType\": \"fermentable\"}},{\"term\": {\"dataType\": \"yeast\"}},{\"term\": {\"dataType\": \"other\"}}]},\"query\": {\"match\": {\"name\": {\"query\": \"" + query +"\"}}}}";
            // var res = await _client.SearchAsync<string>(_index, queryString);
            //return res.Response;
            return "";
        }
    }
}
