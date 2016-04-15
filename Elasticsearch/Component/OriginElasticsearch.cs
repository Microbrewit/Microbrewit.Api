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
    public class OriginElasticsearch : IOriginElasticsearch
    {
        private readonly ElasticSearchSettings _elasticSearchSettings;
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;

        public OriginElasticsearch(IOptions<ElasticSearchSettings> elasticsearchSettings)
        {
            _elasticSearchSettings = elasticsearchSettings.Value;  
            this._node = new Uri(_elasticSearchSettings.Url);
            this._settings = new ConnectionSettings(_node);
            _settings.DefaultIndex(_elasticSearchSettings.Index);
            this._client = new ElasticClient(_settings);
        }

        public async Task UpdateAsync(OriginDto originDto)
        {
            await _client.MapAsync<OriginDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            var index =  await _client.IndexAsync<OriginDto>(originDto);
        }

        public async Task<IEnumerable<OriginDto>> GetAllAsync(int from, int size ,string custom)
        {
            //TODO: Add filter for custom.
            var result = await _client.SearchAsync<OriginDto>(s => s
                .Query(q => q
                .Bool(fi => fi
                    .Filter(f => f.Term(t => t.Type, "origin"))))
                .Size(size)
                .From(from));
            return result.Documents;
        }

        public async Task<OriginDto> GetSingleAsync(int id)
        {
            IGetRequest getRequest = new GetRequest(_elasticSearchSettings.Index, "origin", id.ToString());
            var result = await  _client.GetAsync<OriginDto>(getRequest);
            return result.Source;
        }

        public OriginDto GetSingle(int id)
        {
            IGetRequest getRequest = new GetRequest(_elasticSearchSettings.Index, "origin", id.ToString());
            var result = _client.Get<OriginDto>(getRequest);
            return result.Source;
        }

        public async Task<IEnumerable<OriginDto>> SearchAsync(string query, int @from, int size)
        {
            var searchResults = await _client.SearchAsync<OriginDto>(s => s
                                               .From(from)
                                               .Size(size)
                                               .Query(q => q.Match(m => m.Field(f => f.Name)
                                                                         .Query(query))));
            return searchResults.Documents;
        }

        public async Task UpdateAllAsync(IEnumerable<OriginDto> originDtos)
        {
            await _client.MapAsync<OriginDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            var index = await _client.IndexManyAsync<OriginDto>(originDtos);
        }

        public async Task DeleteAsync(int id)
        {
            await _client.DeleteAsync<OriginDto>(id);
        }
    }
}
