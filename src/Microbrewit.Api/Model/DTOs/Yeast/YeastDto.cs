﻿using System.ComponentModel.DataAnnotations;
using Nest;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    [ElasticsearchType(Name = "yeast",IdProperty = "Id")]
    public class YeastDto : IIngredientDto
    {
        [JsonProperty(PropertyName = "yeastId")]
        public int Id { get; set; }
        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "temperature")]
        public Temperature Temperature { get; set; }
        [JsonProperty(PropertyName = "flocculation")]
        public string Flocculation { get; set; }
        [JsonProperty(PropertyName = "flocculationLow")]
        public int FlocculationLow { get; set; }
        [JsonProperty(PropertyName = "flocculationHigh")]
        public int FlocculationHigh { get; set; }
        [JsonProperty(PropertyName = "alcoholTolerance")]
        public string AlcoholTolerance { get; set; }
        [JsonProperty(PropertyName = "alcoholToleranceLow")]
        public double AlcoholToleranceLow { get; set; }
        [JsonProperty(PropertyName = "alcoholToleranceHigh")]
        public double AlcoholToleranceHigh { get; set; }
        [JsonProperty(PropertyName = "productCode")]
        public string ProductCode { get; set; }
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
        [JsonProperty(PropertyName = "brewerySource")]
        public string BrewerySource { get; set; }
        [JsonProperty(PropertyName = "species")]
        public string Species { get; set; }
        [JsonProperty(PropertyName = "attenutionRange")]
        public string AttenutionRange { get; set; }
        [JsonProperty(PropertyName = "attenutionLow")]
        public int AttenutionLow { get; set; }
        [JsonProperty(PropertyName = "attenutionHigh")]
        public int AttenutionHigh { get; set; }
        [JsonProperty(PropertyName = "pitchingFermentationNotes")]
        public string PitchingFermentationNotes { get; set; }
        [JsonProperty(PropertyName = "supplier")]
        public DTO Supplier { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type => "yeast";

        [Required]
        [JsonProperty(PropertyName = "custom")]
        public bool Custom { get; set; }
        public string SubType {get; set;}
        [JsonProperty("sources")]
        public IEnumerable<SourceDto> Sources { get; set; }
    }
}