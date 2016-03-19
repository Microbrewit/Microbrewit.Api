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
    public class BeerElasticsearch : IBeerElasticsearch
    {
        private ElasticSearchSettings _elasticSearchSettings;
        private readonly Uri _node;
        private readonly ConnectionSettings _settings;
        private readonly ElasticClient _client;
        private const int BigNumber = 10000;

        public BeerElasticsearch(IOptions<ElasticSearchSettings> elasticSearchSettings)
        {
            _elasticSearchSettings = elasticSearchSettings.Value;
            this._node = new Uri(_elasticSearchSettings.Url);
            this._settings = new ConnectionSettings(_node);
            this._client = new ElasticClient(_settings);
        }

        public async Task<IIndexResponse> UpdateAsync(BeerDto beerDto)
        {
            // Adds an analayzer to the name property in FermentableDto object.
            await _client.MapAsync<BeerDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            return await _client.IndexAsync<BeerDto>(beerDto, idx => idx.Index(_elasticSearchSettings.Index));
        }

        public async Task<IEnumerable<BeerDto>> GetAllAsync(int from, int size)
        {
            var res =
             await _client.SearchAsync<BeerDto>(
                 s => s
                     .From(from)
                     .Size(size)
                     .Query(q => q
                        .Filtered(fi => fi
                            .Filter(f => f.Term(h => h.DataType, "beer"))
                     )));
            return res.Documents;
        }

        public async Task<BeerDto> GetSingleAsync(int id)
        {
            IGetRequest getRequest = new GetRequest(_elasticSearchSettings.Index, "beer", id.ToString());
            var result = await _client.GetAsync<BeerDto>(getRequest);
            return result.Source;
        }

        public async Task<IEnumerable<BeerDto>> SearchAsync(string query, int @from, int size)
        {
            var searchResults = await _client.SearchAsync<BeerDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.Match(m => m.Field(f => f.Name)
                                                                          .Query(query))));

            return searchResults.Documents;
        }

        public async Task<IBulkResponse> UpdateAllAsync(IEnumerable<BeerDto> beers)
        {
            await _client.MapAsync<BeerDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            return await _client.IndexManyAsync(beers,_elasticSearchSettings.Index);
        }

        public async Task<IBulkResponse> ReIndexBulk(IEnumerable<BeerDto> beers, string index)
        {
            await _client.MapAsync<BeerDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            return await _client.IndexManyAsync(beers,index);
        }

        public Task<IDeleteResponse> DeleteAsync(int id)
        {
            return _client.DeleteAsync<BeerDto>(id);
        }

        public async Task<IEnumerable<BeerDto>> GetLastAsync(int @from, int size)
        {
            var result = await _client.SearchAsync<BeerDto>(s => s
                                                        .Sort(p => p                                                                                                  
                                                            .Descending(d => d.CreatedDate))
                                                        .From(from)
                                                        .Size(size));
            return result.Documents;
        }

        public async Task<IEnumerable<BeerDto>> GetUserBeersAsync(string username)
        {
            var result = await _client.SearchAsync<BeerDto>(s => s
                .Size(BigNumber)
                .Query(q => q
                .Filtered(f => f
                    .Query(q2 => q2
                    .Term("brewers.username", username))
                    .Filter(filter => filter
                        .Term(t => t.DataType, "beer")
                        ))));
            return result.Documents;
        }

        public async Task<IEnumerable<BeerDto>> GetAllBreweryBeersAsync(int breweryId)
        {
            var result = await _client.SearchAsync<BeerDto>(s => s
                .Size(BigNumber)
                .Query(q => q
                .Filtered(f => f
                    .Query(q2 => q2
                    .Term("breweries.breweryId", breweryId))
                    .Filter(filter => filter
                        .Term(t => t.DataType, "beer")
                        ))));
            return result.Documents;
        }

    }
}
