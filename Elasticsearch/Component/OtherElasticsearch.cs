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
    public class OtherElasticsearch : IOtherElasticsearch
    {
        private readonly ElasticSearchSettings _elasticSearchSettings;
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
        private int _bigNumber = 10000;


        public OtherElasticsearch(IOptions<ElasticSearchSettings> elasticsearchSettings)
        {
            _elasticSearchSettings = elasticsearchSettings.Value;
            this._node = new Uri(_elasticSearchSettings.Url);
            this._settings = new ConnectionSettings(_node);
            _settings.DefaultIndex(_elasticSearchSettings.Index);
            this._client = new ElasticClient(_settings);
        }

        public async Task<IIndexResponse> UpdateAsync(OtherDto otherDto)
        {
            // Adds an analayzer to the name property in FermentableDto object.
            await _client.MapAsync<OtherDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            return await _client.IndexAsync<OtherDto>(otherDto);
        }

        public async Task<IEnumerable<OtherDto>> GetAllAsync(string custom)
        {
            var res =
             await _client.SearchAsync<OtherDto>(
                 s => s
                     .Size(_bigNumber)
                     .Query(q => q
                     .Bool(fi => fi
                        .Filter(f => f.Term(h => h.Type, "other") && f.Term(p => p.Custom, custom)))));
            return res.Documents;
        }

        public async Task<OtherDto> GetSingleAsync(int id)
        {
            IGetRequest getRequest = new GetRequest(_elasticSearchSettings.Index, "other", id.ToString());
            var result = await _client.GetAsync<OtherDto>(getRequest);
            return result.Source;
        }

        public async Task<IEnumerable<OtherDto>> SearchAsync(string query, int @from, int size)
        {
            var searchResults = await _client.SearchAsync<OtherDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.Match(m => m.Field(f => f.Name)
                                                                          .Query(query))));

            return searchResults.Documents;
        }

        public async Task<IBulkResponse> UpdateAllAsync(IEnumerable<OtherDto> others)
        {
            await _client.MapAsync<OtherDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            return await _client.IndexManyAsync(others);
        }

        public Task<IDeleteResponse> DeleteAsync(int id)
        {
            return _client.DeleteAsync<OtherDto>(id);
        }

        public OtherDto GetSingle(int id)
        {
            IGetRequest getRequest = new GetRequest(_elasticSearchSettings.Index, "other", id.ToString());
            var result = _client.Get<OtherDto>(getRequest);
            return result.Source;
        }

        public IEnumerable<OtherDto> Search(string query, int @from, int size)
        {
            var searchResults =  _client.Search<OtherDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.Match(m => m.Field(f => f.Name)
                                                                          .Query(query))));

            return searchResults.Documents;
        }

        public void Update(OtherDto otherDto)
        {
            // Adds an analayzer to the name property in FermentableDto object.
             _client.Map<OtherDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            _client.Index<OtherDto>(otherDto);
        }
    }
}
