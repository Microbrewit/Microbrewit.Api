using System;
using System.Threading.Tasks;
using Microbrewit.Api.ElasticSearch.Interface;
using Nest;

namespace Microbrewit.Api.ElasticSearch.Component
{
    public class SearchElasticsearch : ISearchElasticsearch
    {
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
        private int _bigNumber = 10000;
        private readonly string _url;
        private readonly string _index;

        public SearchElasticsearch()
        {
            _url = "http://localhost:9200";
            this._node = new Uri(_url);
            this._settings = new ConnectionSettings(_node);
            this._client = new ElasticClient(_settings);
            _index = "mb";
        }


        public async Task<string> SearchAllAsync(string query, int @from, int size)
        {
            // var queryString = "{\"from\" : " + from + ", \"size\" : " + size + ", \"query\":{\"match\": {\"name\": {\"query\": \" " + query + " \",\"operator\": \"and\"}}}}";
            //             _client.SearchAsync<string>();
            // var res = await _client.SearchAsync<string>(_index,queryString);
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
