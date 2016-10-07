using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class HopPostMetadataResolver : ValueResolver<HopDto, IEnumerable<Metadata>>
    {
        protected override IEnumerable<Metadata> ResolveCore(HopDto hopDto)
        {
            var metadata = new List<Metadata>();
            foreach (var meta in hopDto.Metadata)
            {
               metadata.Add(new Metadata{Id = hopDto.Id, Key = meta.Key, Value = meta.Value});
            }
            return metadata;
        }
    }
}