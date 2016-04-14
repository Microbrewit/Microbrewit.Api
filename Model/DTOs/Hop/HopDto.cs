using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nest;

namespace Microbrewit.Api.Model.DTOs
{
    [ElasticsearchType(Name= "hop")]
    public class HopDto : IIngredientDto
    {
        [JsonProperty(PropertyName = "hopId")]
        public int Id { get; set; }
        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "acids")]
        public AcidDto Acids { get; set; }
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
        [JsonProperty(PropertyName = "flavourDescription")]
        public String FlavourDescription { get; set; }
        [JsonProperty(PropertyName = "origin")]
        public DTO Origin { get; set; }
        [JsonProperty(PropertyName = "flavours")]
        public IList<string> Flavours { get; set; }
        [JsonProperty(PropertyName = "aromaWheel")]
        public IList<string> AromaWheel { get; set; }
        [JsonProperty(PropertyName = "aliases")]
        public IList<string> Aliases { get; set; }
        [Required]
        [JsonProperty(PropertyName = "purpose")]
        public string Purpose { get; set; }
        [JsonProperty(PropertyName = "substitutes")]
        public IList<DTO> Substituts { get; set; }
        [JsonProperty(PropertyName = "oils")]
        public OilDto Oils { get; set; }
        [JsonProperty(PropertyName = "beerstyles")]
        public IEnumerable<DTO> BeerStyles { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type => "hop";
            
        [Required]
        [JsonProperty(PropertyName = "custom")]
        public bool Custom { get; set; }
        public string SubType {get; set;}
        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate { get; set; }
        [JsonProperty(PropertyName = "updatedDate")]
        public DateTime UpdatedDate { get; set; }
    }
}