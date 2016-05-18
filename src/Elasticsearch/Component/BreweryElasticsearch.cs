using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microbrewit.Api.Elasticsearch.Interface;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;

namespace Microbrewit.Api.Elasticsearch.Component
{
    public class BreweryElasticsearch : IBreweryElasticsearch
    {
        private readonly ElasticSearchSettings _elasticSearchSettings;
        private readonly ILogger<BreweryElasticsearch> _logger;
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
        private const int BigNumber = 10000;
        

        public BreweryElasticsearch(IOptions<ElasticSearchSettings> elasticsearchSettings, ILogger<BreweryElasticsearch> logger)
        {
            _elasticSearchSettings = elasticsearchSettings.Value;
            this._node = new Uri(_elasticSearchSettings.Url);
            this._settings = new ConnectionSettings(_node);
            this._client = new ElasticClient(_settings);
            _logger = logger;
        }

        public async Task UpdateAsync(BreweryDto breweryDto)
        {
            await _client.MapAsync<BreweryDto>(d => d.Properties(p => p
                .String(s => s.Name(n => n.Name).Analyzer("autocomplete"))
                ));
            await _client.IndexAsync(breweryDto, idx => idx.Index(_elasticSearchSettings.Index));
        }

        public async Task<IEnumerable<BreweryDto>> GetAllAsync(int from, int size, bool? isCommercial,string origin,bool? hasBeers)
        {
            //var res = await _client.SearchAsync<BreweryDto>(s => s
            //    .Size(size)
            //    .From(from)
            //    .Query(q => q
            //        .Bool(fi => fi
            //            .Filter(f => f.Term(t => t.Type, "brewery")))));
            //return res.Documents;
            var query = Query<BreweryDto>.Bool(fi => fi.Filter(f => f.Term(t => t.Type, "brewery")));
            if (isCommercial != null)
                query = query && Query<BreweryDto>.Term("isCommercial", isCommercial);
            if(origin != null)
            {
                _logger.LogInformation($"Brewery origin {origin}");
                query = query && Query<BreweryDto>.Match(m => m.Field(f => f.Origin.Name).Query(origin));
            }
            if (hasBeers != null)
            {
                if (hasBeers.Value)
                    query = query && Query<BreweryDto>.Exists(e => e.Field(f => f.Beers));
                else
                    query = query && Query<BreweryDto>.Bool(b => b.MustNot(m => m.Exists(e => e.Field(f => f.Beers))));
            }
            var res = await _client.SearchAsync<BreweryDto>(new SearchRequest()
            {
                From = from,
                Size = size,
                Query = query,
            });
            return res.Documents;
        }

        public async Task<BreweryDto> GetSingleAsync(int id)
        {
            IGetRequest getRequest = new GetRequest(_elasticSearchSettings.Index, "brewery", id.ToString());
            var result = await _client.GetAsync<BreweryDto>(getRequest);
            return (BreweryDto)result.Source;
        }

        public async Task<IEnumerable<BreweryDto>> SearchAsync(string query, int from, int size)
        {
            var fields = new List<string> { "name" };
            var searchResults = await _client.SearchAsync<BreweryDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.Match(m => m.Field(f => f.Name).Query(query))));
            return searchResults.Documents;
        }

        public async Task UpdateAllAsync(IEnumerable<BreweryDto> brewerys)
        {
            await _client.MapAsync<BreweryDto>(d => d.Properties(p => p
               .String(s => s.Name(n => n.Name).Analyzer("autocomplete"))
               ));
            await _client.IndexManyAsync(brewerys,_elasticSearchSettings.Index);
        }

        public async Task DeleteAsync(int id)
        {
            await _client.DeleteAsync<BreweryDto>(id);
        }

        public async Task<IEnumerable<BreweryMemberDto>> GetAllMembersAsync(int breweryId)
        {
            var res = await GetSingleAsync(breweryId);
            return res.Members;
        }

        public async Task<BreweryMemberDto> GetSingleMemberAsync(int breweryId, string username)
        {
            var result = await GetSingleAsync(breweryId);
            return result.Members.SingleOrDefault(m => m.UserId.Equals(username));
        }

        public IEnumerable<BreweryMemberDto> GetMemberships(string userId)
        {
            var breweryDto =_client.Search<BreweryDto>(s => s
                .Size(BigNumber)
                .Query(q => q
                    .Bool(f => f
                        .Must(qu => qu.MatchAll())
                        .Filter(fi => fi
                            .Nested(n => n
                               .Path("members")
                            //    .Filter(fl => fl
                            //         .Bool(b => b
                            //             .Must(m => m
                            //                 .Term("username",username)
                       // )))
                        )))));
            var breweryMemberDtos = from brewery in breweryDto.Documents
                from member in brewery.Members
                where member.UserId.Equals(userId)
                select member;
            return breweryMemberDtos;
        }
    }

}
