﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.ElasticSearch.Interface;
using Microbrewit.Api.Model.DTOs;
using Nest;

namespace Microbrewit.Api.ElasticSearch.Component
{
    public class GlassElasticsearch : IGlassElasticsearch
    {
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
        private int _bigNumber = 10000;

        public GlassElasticsearch()
        {
            string url = "http://localhost:9200";
            this._node = new Uri(url);
            this._settings = new ConnectionSettings(_node);
            this._client = new ElasticClient(_settings);
        }

        public async Task<IIndexResponse> UpdateAsync(GlassDto glassDto)
        {
            // Adds an analayzer to the name property in FermentableDto object.
            await _client.MapAsync<GlassDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            return await _client.IndexAsync<GlassDto>(glassDto);
        }

        public async Task<IEnumerable<GlassDto>> GetAllAsync()
        {
            var res =
             await _client.SearchAsync<GlassDto>(
                 s => s
                     .Size(_bigNumber)
                     .Query(q => q
                        .Filtered(fi => fi
                            .Filter(f => f.Term(h => h.DataType, "glass")))));
            return res.Documents;
        }

        public async Task<GlassDto> GetSingleAsync(int id)
        {
            IGetRequest getRequest = new GetRequest("mb", "glass", id.ToString());
            var result = await _client.GetAsync<GlassDto>(getRequest);
            return result.Source;
        }

        public async Task<IEnumerable<GlassDto>> SearchAsync(string query, int @from, int size)
        {
            var searchResults = await _client.SearchAsync<GlassDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q => q.Match(m => m.Field(f => f.Name)
                                                                          .Query(query))));

            return searchResults.Documents;
        }

        public async Task<IBulkResponse> UpdateAllAsync(IEnumerable<GlassDto> glasss)
        {
            await _client.MapAsync<GlassDto>(d => d.Properties(p => p.String(s => s.Name(n => n.Name).Analyzer("autocomplete"))));
            return await _client.IndexManyAsync(glasss);
        }

        public Task<IDeleteResponse> DeleteAsync(int id)
        {
            return _client.DeleteAsync<GlassDto>(id);
        }
    }
}
