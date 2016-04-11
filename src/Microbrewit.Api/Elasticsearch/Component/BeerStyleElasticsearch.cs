using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.ElasticSearch.Interface;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Settings;
using Microsoft.Extensions.OptionsModel;
using Nest;

namespace Microbrewit.Api.ElasticSearch.Component
{
    public class BeerStyleElasticsearch : IBeerStyleElasticsearch
    {
        private readonly ElasticSearchSettings _elasticSearchSettings;
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
        private const int BigNumber = 10000;

        public BeerStyleElasticsearch(IOptions<ElasticSearchSettings> elasticsearchSettings)
        {
            _elasticSearchSettings = elasticsearchSettings.Value;
            this._node = new Uri(_elasticSearchSettings.Url);
            this._settings = new ConnectionSettings(_node);
            this._client = new ElasticClient(_settings);
        }

        public async Task UpdateAsync(BeerStyleDto beerStyleDto)
        {
            await _client.MapAsync<BeerStyleDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            var index = await _client.IndexAsync<BeerStyleDto>(beerStyleDto, idx => idx.Index(_elasticSearchSettings.Index));
        }

        public async Task<IEnumerable<BeerStyleDto>> GetAllAsync(int from, int size)
        {
            var result = await _client.SearchAsync<BeerStyleDto>(s => s
                .Query(q => q
                    .Filtered(fi => fi
                        .Filter(f => f.Term(t => t.Type, "beerstyle"))))
                .Size(size)
                .From(from)
                );
            return result.Documents;
        }

        public async Task<BeerStyleDto> GetSingleAsync(int id)
        {
            IGetRequest getRequest = new GetRequest(_elasticSearchSettings.Index, "beerStyle", id.ToString());
            var result = await _client.GetAsync<BeerStyleDto>(getRequest);
            return result.Source;
        }

        public async Task<IEnumerable<BeerStyleDto>> SearchAsync(string query, int @from, int size)
        {
            var searchResults = await _client.SearchAsync<BeerStyleDto>(s => s
                                               .From(from)
                                               .Size(size)
                                               .Query(q => q.Match(m => m.Field(f => f.Name)
                                                                         .Query(query))));
            return searchResults.Documents;
        }

        public async Task UpdateAllAsync(IEnumerable<BeerStyleDto> beerStyleDtos)
        {
            await _client.MapAsync<BeerStyleDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            var index = await _client.IndexManyAsync<BeerStyleDto>(beerStyleDtos,_elasticSearchSettings.Index);
        }

        public async Task DeleteAsync(int id)
        {
            await _client.DeleteAsync<BeerStyleDto>(id);
        }

        public BeerStyleDto GetSingle(int id)
        {
            IGetRequest getRequest = new GetRequest(_elasticSearchSettings.Index, "beerStyle", id.ToString());
            var result = _client.Get<BeerStyleDto>(getRequest);
            return result.Source;
        }

        public IEnumerable<BeerStyleDto> Search(string query, int @from, int size)
        {
            var searchResults =  _client.Search<BeerStyleDto>(s => s
                                               .From(from)
                                               .Size(size)
                                               .Query(q => q.Match(m => m.Field(f => f.Name)
                                                                         .Query(query))));
            return searchResults.Documents;
        }
    }
}
