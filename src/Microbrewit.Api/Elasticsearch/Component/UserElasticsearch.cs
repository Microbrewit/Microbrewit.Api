using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Elasticsearch.Interface;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;

namespace Microbrewit.Api.Elasticsearch.Component
{
    public class UserElasticsearch : IUserElasticsearch
    {
        private readonly ElasticSearchSettings _elasticSearchSettings;
        private readonly ILogger<UserElasticsearch> _logger; 
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
        private string _index;

        public UserElasticsearch(IOptions<ElasticSearchSettings> elsaticSearchSettings, ILogger<UserElasticsearch> logger)
        {
            _logger = logger;
            _elasticSearchSettings = elsaticSearchSettings.Value;
            this._node = new Uri(_elasticSearchSettings.Url);
            this._settings = new ConnectionSettings(_node);
            this._client = new ElasticClient(_settings);
            _index = "mb";
        }

        public async Task<IIndexResponse> UpdateAsync(UserDto userDto)
        {
            try
            {

            // Adds an analayzer to the name property in FermentableDto object.
            await _client.MapAsync<UserDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Username).Analyzer("autocomplete"))));
            return await _client.IndexAsync<UserDto>(userDto);

            }
            catch (Exception exception)
            {
                _logger.LogError(exception.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync(int from, int size)
        {
            var result = await _client.SearchAsync<UserDto>(s => s
                                                .Query(q => q
                                                .Bool(fi => fi
                                                    .Filter(f => f.Term(t => t.Type, "user"))
                                                    .Filter(f => f.Term(t => t.EmailConfirmed, "true"))))
                                                .Size(size)
                                                .From(from)
                                                );
            return result.Documents;
        }

        public async Task<UserDto> GetSingleAsync(string username)
        {
            IGetRequest getRequest = new GetRequest(_index, "user", username);
            var result = await _client.GetAsync<UserDto>(getRequest);
            return result.Source;
        }

        public async Task<IEnumerable<UserDto>> SearchAsync(string query, int @from, int size)
        {
            var searchResults = await _client.SearchAsync<UserDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.Match(m => m.Field(f => f.Username)
                                                                          .Query(query))));

            return searchResults.Documents;
        }

        public async Task<IBulkResponse> UpdateAllAsync(IEnumerable<UserDto> users)
        {
            await _client.MapAsync<UserDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Username).Analyzer("autocomplete"))));
            return await _client.IndexManyAsync(users);
        }

        public Task<IDeleteResponse> DeleteAsync(string username)
        {
            return _client.DeleteAsync<UserDto>(username);
        }
    }
}
