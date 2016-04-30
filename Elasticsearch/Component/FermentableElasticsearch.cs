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
    public class FermentableElasticsearch : IFermentableElasticsearch
    {
        private readonly ElasticSearchSettings _elasticSearchSettings;
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
        private int _bigNumber = 10000;

        public FermentableElasticsearch(IOptions<ElasticSearchSettings> elasticsearchSettings)
        {
            _elasticSearchSettings = elasticsearchSettings.Value;
            this._node = new Uri(_elasticSearchSettings.Url);
            this._settings = new ConnectionSettings(_node);
            this._client = new ElasticClient(_settings);
        }

        public async Task UpdateAsync(FermentableDto fermentableDto)
        {
            await _client.MapAsync<FermentableDto>(d => d.Properties(p => p
                .String(s => s.Name(n => n.Name).Analyzer("autocomplete"))
                ));
            await _client.IndexAsync(fermentableDto,idx => idx.Index(_elasticSearchSettings.Index));
        }

        public async Task<IEnumerable<FermentableDto>> GetAllAsync(int from,int size,string custom)
        {
            var res = await _client.SearchAsync<FermentableDto>(s => s
                .Size(size)
                .From(from)
                .Query(q => q
                    .Bool(fi => fi
                        .Filter(f => f.Term(t => t.Type, "fermentable") && f.Term(t => t.Custom, custom)))));
            return res.Documents;
        }

        public async Task<FermentableDto> GetSingleAsync(int id)
        {
            IGetRequest getRequest = new GetRequest("mb", "fermentable", id.ToString());
            var result = await _client.GetAsync<FermentableDto>(getRequest);
            return (FermentableDto)result.Source;
        }

        public async Task<IEnumerable<FermentableDto>> SearchAsync(string query, int from, int size)
        {
            var searchResults = await _client.SearchAsync<FermentableDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.Match(m => m.Field("name").Query(query))));
            return searchResults.Documents;
        }

        public async Task UpdateAllAsync(IEnumerable<FermentableDto> fermentableDtos)
        {
            await _client.MapAsync<FermentableDto>(d => d.Properties(p => p
               .String(s => s.Name(n => n.Name).Analyzer("autocomplete"))
               ));
            await _client.IndexManyAsync(fermentableDtos,_elasticSearchSettings.Index);
        }

        public async Task DeleteAsync(int id)
        {
            await _client.DeleteAsync<FermentableDto>(id);
        }

        public FermentableDto GetSingle(int id)
        {
            IGetRequest getRequest = new GetRequest("mb", "fermentable", id.ToString());
            var result = _client.Get<FermentableDto>(getRequest);
            return (FermentableDto)result.Source;
        }

        public IEnumerable<FermentableDto> GetAll(string custom)
        {
            var res = _client.Search<FermentableDto>(s => s
                                                        .Size(_bigNumber)
                                                        //.Filter(f => f
                                                          //  .Term(t => t
                                                           //     .DataType, "fermentable") && f.Term(t => t.Custom, custom))
                                                                );
            return res.Documents;
        }

        public IEnumerable<FermentableDto> Search(string query, int @from, int size)
        {
            //var fields = new List<string> { "name" };
            var searchResults = _client.Search<FermentableDto>(s => s
                .From(from)
                .Size(size)
                .Query(q => q.Match(m => m.Field("name").Query(query))));

            return searchResults.Documents;
        }
    }

}
