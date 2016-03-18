﻿using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    // [ElasticType(Name = "supplier")]
    public class SupplierDto
    {
        [JsonProperty(PropertyName = "supplierId")]
        public int Id { get; set; }
        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "origin")]
        public DTO Origin { get; set; }
        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get { return "supplier"; } }


    }
}