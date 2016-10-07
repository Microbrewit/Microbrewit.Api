using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class HopMetadataResolver : ValueResolver<Hop, IDictionary<string,string>>
    {
        protected override IDictionary<string,string> ResolveCore(Hop hop)
        {
            var metadatas = new Dictionary<string,string>();
            foreach (var metadata in hop.Metadata)
            {
                metadatas.Add(metadata.Key,metadata.Value);
            }
            return metadatas;
        }
    }
}