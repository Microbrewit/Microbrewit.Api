using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Elasticsearch.Interface;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Settings;
using Microsoft.Extensions.Options;
using Nest;

namespace Microbrewit.Api.Elasticsearch.Component
{
    public class HopElasticsearch : Interface.IHopElasticsearch
    {
        private readonly ElasticSearchSettings _elasticSearchSettings;
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;

        public HopElasticsearch(IOptions<ElasticSearchSettings> elasticsearchSettings)
        {
            _elasticSearchSettings = elasticsearchSettings.Value;
            this._node = new Uri(_elasticSearchSettings.Url);
            this._settings = new ConnectionSettings(_node);
            this._settings.DefaultIndex(_elasticSearchSettings.Index);
            this._client = new ElasticClient(_settings);
        }

        public async Task UpdateAllAsync(IEnumerable<HopDto> hops)
        {
            foreach (var hop in hops)
            {
                await UpdateAsync(hop);
            }
        }

        public async Task UpdateAsync(HopDto hop)
        {
            // Adds an analayzer to the name property in FermentableDto object.
            await _client.MapAsync<HopDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            await _client.IndexAsync<HopDto>(hop);
            
            
        }

        public async Task<IEnumerable<HopDto>> GetAllAsync(int from, int size)
        {
            var res =
               await _client.SearchAsync<HopDto>(
                    s => s
                        .From(from)
                        .Size(size)
                        .Query(q => q
                            .Bool(fd => fd
                                .Filter(f => f
                                    .Term(h => h.Type, "hop")
                                    ))));
            return res.Documents;
        }

        public async Task<HopDto> GetSingleAsync(int id)
        {
            IGetRequest getRequest = new GetRequest(_elasticSearchSettings.Index, "hop", id.ToString());
            var result = await _client.GetAsync<HopDto>(getRequest);
            return result.Source;
        }


        public async Task<IEnumerable<HopDto>> SearchAsync(string query, int from, int size)
        {
            var searchResults = await _client.SearchAsync<HopDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q1 => q1
                                                .Bool(fi => fi                                              
                                                    .Filter(f => f.Term(t => t.Type, "hop"))
                                                .Must(q => q.Match(m => m.Field(f => f.Name)
                                                                          .Query(query))))));

            return searchResults.Documents;
        }

        public async Task DeleteAsync(int id)
        {
            await _client.DeleteAsync<HopDto>(id);
        }

        public HopDto GetSingle(int id)
        {
            IGetRequest getRequest = new GetRequest(_elasticSearchSettings.Index, "hop", id.ToString());
            var result = _client.Get<HopDto>(getRequest);
            return result.Source;
        }

        public IEnumerable<HopDto> Search(string query, int @from, int size)
        {
            var searchResults = _client.Search<HopDto>(s => s
                                               .From(from)
                                               .Size(size)
                                               .Query(q1 => q1
                                               .Bool(fi => fi
                                                .Filter(f => f.Term(t => t.Type, "hop"))
                                                .Must(q2 => q2.Match(m => m.Field(f => f.Name)
                                                                         .Query(query))))));

            return searchResults.Documents;
        }

        public async Task UpdateAromaWheelAsync(AromaWheelDto aromaWheelDto)
        {
            await _client.MapAsync<AromaWheelDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            await _client.IndexAsync<AromaWheelDto>(aromaWheelDto);    
        }

        public async Task UpdateAllAromaWheelAsync(IEnumerable<AromaWheelDto> aromaWheels)
        {
            foreach (var aromaWheel in aromaWheels)
            {
                await UpdateAromaWheelAsync(aromaWheel);
            }
        }

        public async Task<IEnumerable<AromaWheelDto>> GetAromaWheelsAsync()
        {
            
            var res =
               await _client.SearchAsync<AromaWheelDto>(
                    s => s
                        .Size(10000)
                        .Query(q => q
                            .Bool(fd => fd
                                .Filter(f => f
                                    .Term(h => h.Type, "aromawheel")
                                    ))));
            return res.Documents;
        }

        public async Task<IEnumerable<HopDto>> GetHopsByAromaWheel(string aromaWheel)
        {
             var searchResults = await _client.SearchAsync<HopDto>(s => s
                                                .From(0)
                                                .Size(10000)
                                                .Query(q1 => q1
                                                    .Bool(fi => fi
                                                        .Must(q2 => q2.Match(m => m.Field(f => f.AromaWheels).Query("Cirus"))
                                                                         ))));

        var searchTerm = new TermQuery 
        {
            Field= "aromawheel.name",
            Value = aromaWheel,
        };
        var result = await _client.SearchAsync<HopDto>(new SearchRequest<HopDto>{Query= searchTerm});
            return searchResults.Documents;
        }
    }
}
