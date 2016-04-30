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
    public class SupplierElasticsearch : ISupplierElasticsearch
    {
        private readonly ElasticSearchSettings _elasticSearchSettings;
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
        private int _bigNumber = 10000;
        public SupplierElasticsearch(IOptions<ElasticSearchSettings> elasticsearchSettings)
        {
            _elasticSearchSettings = elasticsearchSettings.Value;
            this._node = new Uri(_elasticSearchSettings.Url);
            this._settings = new ConnectionSettings(_node);
            _settings.DefaultIndex(_elasticSearchSettings.Index);
            this._client = new ElasticClient(_settings);
        }

        public async Task UpdateAsync(SupplierDto supplierDto)
        {
            // Adds an analayzer to the name property in FermentableDto object.
            await _client.MapAsync<SupplierDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            var index = await _client.IndexAsync<SupplierDto>(supplierDto);
        }

        public async Task<IEnumerable<SupplierDto>> GetAllAsync(string custom)
        {
            //TODO: add custom to supplier.
            var res = await _client.SearchAsync<SupplierDto>(s => s
                .Size(_bigNumber)
                .Query(q => q
                    .Bool(fi => fi
                        .Filter(f => f.Term(t => t.Type, "supplier")))));
            return res.Documents;


        }

        public async Task<SupplierDto> GetSingleAsync(int id)
        {
            IGetRequest getRequest = new GetRequest(_elasticSearchSettings.Index, "supplier", id.ToString());
            var result = await _client.GetAsync<SupplierDto>(getRequest);
            return result.Source;
        }

        public async Task<IEnumerable<SupplierDto>> SearchAsync(string query, int @from, int size)
        {
            var searchResults = await _client.SearchAsync<SupplierDto>(s => s
                                               .From(from)
                                               .Size(size)
                                               .Query(q => q.Match(m => m.Field(f => f.Name)
                                                                         .Query(query))));
            return searchResults.Documents;
        }

        public async Task UpdateAllAsync(IEnumerable<SupplierDto> supplierDtos)
        {
            await _client.MapAsync<SupplierDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            var index = await _client.IndexManyAsync<SupplierDto>(supplierDtos);
        }

        public async Task DeleteAsync(int id)
        {
            await _client.DeleteAsync<SupplierDto>(id);
        }
    }
}
