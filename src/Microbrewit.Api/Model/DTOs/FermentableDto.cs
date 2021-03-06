﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Nest;
using System;

namespace Microbrewit.Api.Model.DTOs
{
    [ElasticsearchType(Name = "fermentable")]
    public class FermentableDto : IIngredientDto
    {
        [JsonProperty(PropertyName = "fermentableId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "supplier")]
        public SupplierDto Supplier { get; set; }
        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "lovibond")]
        public double Lovibond { get; set; }
        [JsonProperty(PropertyName = "ppg")]
        public int PPG { get; set; }
        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; }
        [Required]
        [JsonProperty(PropertyName = "subType")]
        public string SubType { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type => "fermentable";
        [Required]
        [JsonProperty(PropertyName = "custom")]
        public bool Custom { get; set; }
        [JsonProperty(PropertyName = "mustMash")]
        public bool MustMash { get; set; }
        [JsonProperty(PropertyName = "maxInBatch")]
        public int MaxInBatch { get; set; }
        [JsonProperty(PropertyName = "protein")]
        public decimal Protein { get; set; }
        [JsonProperty(PropertyName = "diastaticPower")]
        public decimal DiastaticPower { get; set; }
        [JsonProperty(PropertyName = "addAfterBoil")]
        public bool AddAfterBoil { get; set; }
        [JsonProperty(PropertyName = "moisture")]
        public decimal Moisture { get; set; }
        [JsonProperty(PropertyName = "coarseFineDiff")]
        public decimal CoarseFineDiff { get; set; }
        [JsonProperty(PropertyName = "dryYield")]
        public decimal DryYield { get; set; }

        [JsonProperty(PropertyName = "superFermentableId")]
        public int? SuperFermentableId { get; set; }
        [JsonProperty(PropertyName = "subFermentables")]
        public IList<DTO> SubFermentables { get; set; }
        [JsonProperty(PropertyName = "flavours")]
        public IList<string> Flavours { get; set; }
        [JsonProperty(PropertyName = "createdDate")]
        public DateTimeOffset CreatedDate { get; set; }
        [JsonProperty(PropertyName = "updatedDate")]
        public DateTimeOffset UpdatedDate { get; set; }
        [JsonProperty("sources")]
        public IEnumerable<SourceDto> Sources { get; set; }
    }

}