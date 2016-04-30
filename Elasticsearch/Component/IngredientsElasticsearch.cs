using Microbrewit.Api.Elasticsearch.Interface;
using Microbrewit.Api.Settings;
using Microsoft.Extensions.OptionsModel;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microbrewit.Api.Elasticsearch.Component
{
    public class IngredientElasticsearch : IIngredientElasticsearch
    {
        private readonly ElasticSearchSettings _elasticSearchSettings;
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;

        public IngredientElasticsearch(IOptions<ElasticSearchSettings> elasticsearchSettings)
        {
            _elasticSearchSettings = elasticsearchSettings.Value;
            this._node = new Uri(_elasticSearchSettings.Url);
            this._settings = new ConnectionSettings(_node);
            this._client = new ElasticClient(_settings);
        }

        public async Task<bool> HasIngredientsBeenUpdated(DateTime time)
        {
            var ingredients = new[] { "hop", "other", "fermentable", "yeast" };
            var boolQuery = Query<dynamic>.Bool(b => b.Filter(f => f.Terms(t => t.Field("type").Terms(ingredients))).Filter(m => m.DateRange(dr => dr.Field("updatedDate").GreaterThan(time))));
            var res = await _client.SearchAsync<dynamic>(new SearchRequest()
            {
                Size = 1,
                Query = boolQuery
            });
            return res.Documents.Any();
        }

        public async Task<IEnumerable<dynamic>> SearchIngredientsAsync(string query, int @from, int size)
        {
            var ingredients = new[] { "hop", "other", "fermentable", "yeast" };
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
