﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.ElasticSearch.Interface;
using Microbrewit.Api.Model.DTOs;
using Nest;

namespace Microbrewit.Api.ElasticSearch.Component
{
    public class HopElasticsearch : IHopElasticsearch
    {
        private Uri _node;
        private ConnectionSettings _settings;
        private ElasticClient _client;
        private string _index;
        private int _bigNumber = 10000;

        public HopElasticsearch()
        {
            this._index = "mb";
            string url = "http://localhost:9200";
            this._node = new Uri(url);
            this._settings = new ConnectionSettings(_node);
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
                            .Filtered(fd => fd
                                .Filter(f => f
                                    .Term(h => h.DataType, "hop")
                                    ))));
            return res.Documents;
        }

        public async Task<HopDto> GetSingleAsync(int id)
        {
            IGetRequest getRequest = new GetRequest(_index, "hop", id.ToString());
            var result = await _client.GetAsync<HopDto>(getRequest);
            return result.Source;
        }


        public async Task<IEnumerable<HopDto>> SearchAsync(string query, int from, int size)
        {
            var searchResults = await _client.SearchAsync<HopDto>(s => s
                                                .From(from)
                                                .Size(size)
                                                .Query(q1 => q1
                                                .Filtered(fi => fi                                              
                                                    .Filter(f => f.Term(t => t.DataType, "hop"))
                                                .Query(q => q.Match(m => m.Field(f => f.Name)
                                                                          .Query(query))))));

            return searchResults.Documents;
        }

        public async Task DeleteAsync(int id)
        {
            await _client.DeleteAsync<HopDto>(id);
        }

        public HopDto GetSingle(int id)
        {
            IGetRequest getRequest = new GetRequest(_index, "hop", id.ToString());
            var result = _client.Get<HopDto>(getRequest);
            return result.Source;
        }

        public IEnumerable<HopDto> Search(string query, int @from, int size)
        {
            var searchResults = _client.Search<HopDto>(s => s
                                               .From(from)
                                               .Size(size)
                                               .Query(q1 => q1
                                               .Filtered(fi => fi
                                                .Filter(f => f.Term(t => t.DataType, "hop"))
                                                .Query(q2 => q2.Match(m => m.Field(f => f.Name)
                                                                         .Query(query))))));

            return searchResults.Documents;
        }

    }
}
