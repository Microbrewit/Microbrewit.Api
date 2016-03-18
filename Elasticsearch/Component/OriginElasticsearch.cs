using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.ElasticSearch.Interface;
using Microbrewit.Api.Model.DTOs;
using Nest;

namespace Microbrewit.Api.ElasticSearch.Component
{
    public class OriginElasticsearch : IOriginElasticsearch
    {
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
        private int _bigNumber = 10000;
        private readonly string _url;
        private string _index;

        public OriginElasticsearch()
        {
            this._url = "http://localhost:9200";          
            this._node = new Uri(_url);
            this._settings = new ConnectionSettings(_node);
            this._client = new ElasticClient(_settings);
            this._index = "mb";
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
                .Filtered(fi => fi
                    .Filter(f => f.Term(t => t.DataType, "origin"))))
                .Size(size)
                .From(from)
                );
            return result.Documents;
        }

        public async Task<OriginDto> GetSingleAsync(int id)
        {
            IGetRequest getRequest = new GetRequest(_index, "origin", id.ToString());
            var result = await  _client.GetAsync<OriginDto>(getRequest);
            return result.Source;
        }

        public OriginDto GetSingle(int id)
        {
            IGetRequest getRequest = new GetRequest(_index, "origin", id.ToString());
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
