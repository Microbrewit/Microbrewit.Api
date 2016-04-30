using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Elasticsearch.Interface;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Settings;
using Microsoft.Extensions.OptionsModel;
using Nest;

namespace Microbrewit.Api.Elasticsearch.Component
{
    public class YeastElasticsearch : IYeastElasticsearch
    {

        private readonly ElasticSearchSettings _elasticSearchSettings;
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
        private int _bigNumber = 10000;

        public YeastElasticsearch(IOptions<ElasticSearchSettings> elasticSearchSettings)
        {
            _elasticSearchSettings = elasticSearchSettings.Value;
            this._node = new Uri( _elasticSearchSettings.Url);
            this._settings = new ConnectionSettings(_node);
            _settings.DefaultIndex(_elasticSearchSettings.Index);
            this._client = new ElasticClient(_settings);
        }

        public async Task UpdateAsync(YeastDto yeastDto)
        {
            await _client.MapAsync<YeastDto>(d => d.Properties(p => p
                .String(s => s.Name(n => n.Name).Analyzer("autocomplete"))
                .String(s => s.Name(n => n.ProductCode).Analyzer("autocomplete"))
                ));
            await _client.IndexAsync(yeastDto);
        }

        public async Task<IEnumerable<YeastDto>> GetAllAsync(string custom)
        {
            var res = await _client.SearchAsync<YeastDto>(s => s
                .Size(_bigNumber)
                .Query(q => q
                    .Bool(fi => fi
                    .Filter(f => f.Term(t => t.Type, "yeast") && f.Term(t => t.Custom, custom)))));
            return res.Documents;
        }

        public async Task<YeastDto> GetSingleAsync(int id)
        {
            IGetRequest getRequest = new GetRequest( _elasticSearchSettings.Index, "yeast", id.ToString());
            var result = await _client.GetAsync<YeastDto>(getRequest);
            return (YeastDto)result.Source;
        }

        public async Task<IEnumerable<YeastDto>> SearchAsync(string query, int from, int size)
        {
            var fields = new List<string> { "name", "productCode" };
            var searchResults = await _client.SearchAsync<YeastDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.Match(m => m.Field(f => f.Name).Query(query))));
            return searchResults.Documents;
        }

        public async Task UpdateAllAsync(IEnumerable<YeastDto> yeasts)
        {
            await _client.MapAsync<YeastDto>(d => d.Properties(p => p
               .String(s => s.Name(n => n.Name).Analyzer("autocomplete"))
               .String(s => s.Name(n => n.ProductCode).Analyzer("autocomplete"))
               ));
            await _client.IndexManyAsync(yeasts);
        }

        public async Task DeleteAsync(int id)
        {
            await _client.DeleteAsync<YeastDto>(id);
        }
    }

}
