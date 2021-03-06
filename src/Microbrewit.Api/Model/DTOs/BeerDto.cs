﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nest;

namespace Microbrewit.Api.Model.DTOs
{
    [ElasticsearchType(Name = "beer")]
    public class BeerDto
    {
            [JsonProperty(PropertyName = "id")]
            public int Id { get; set; }
            [Required]
            [StringLength(500,MinimumLength = 3)]
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
            [JsonProperty(PropertyName = "abv")]
            public ABVDto ABV { get; set; }
            [JsonProperty(PropertyName = "ibu")]
            public IBUDto IBU { get; set; }
            [JsonProperty(PropertyName = "srm")]
            public SRMDto SRM { get; set; }
            [JsonProperty(PropertyName = "forkOfId")]
            public int? ForkOfId { get; set; }
            [JsonProperty(PropertyName = "forkOf")]
            public BeerSimpleDto ForkOf { get; set; }
            [Required]
            [JsonProperty(PropertyName = "beerStyle")]
            public BeerStyleSimpleDto BeerStyle { get; set; }
            [JsonProperty(PropertyName = "recipe")]
            public RecipeDto Recipe { get; set; }
            [JsonProperty(PropertyName = "createdDate")]
            public DateTime CreatedDate { get; set; }
            [JsonProperty(PropertyName = "updatedDate")]
            public DateTime UpdatedDate { get; set; }
            [JsonProperty(PropertyName = "isCommercial")]
            public bool IsCommercial { get; set; }
            [JsonProperty(PropertyName = "breweries")]
            public IList<BrewerySimpleDto> Breweries { get; set; }
            [JsonProperty(PropertyName = "brewers")]
            public IList<DTOUser> Brewers { get; set; }
            [JsonProperty(PropertyName = "type")]
            public string DataType => "beer";
            [JsonProperty(PropertyName = "forks")]
            public IList<BeerSimpleDto> Forks { get; set; }
       
    }
}
