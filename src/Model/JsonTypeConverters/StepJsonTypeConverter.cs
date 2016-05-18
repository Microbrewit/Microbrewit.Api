using System;
using System.Collections.Generic;
using Microbrewit.Api.Model.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microbrewit.Api.Model.JsonTypeConverters
{
    public class StepsJsonTypeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
 
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var steps = new List<IStepDto>();
            if (reader.TokenType != JsonToken.StartArray) return steps;
 
            var jsonArray = JArray.Load(reader);
            foreach (var jObject in jsonArray.Children())
            {
               var step = GetValue(jObject);
                if(step != null)
                    steps.Add(step);
            }
            return steps;
        }
 
        private static IStepDto GetValue(JToken jObject)
        {
            if (!jObject.HasValues) return null;
            var type = jObject["type"];
            if (type == null) return null;
            switch (type.ToString())
            {
                case "mash":
                    return jObject.ToObject<MashStepDto>();
                case "boil":
                    return jObject.ToObject<BoilStepDto>();
                case "fermentation":
                   return jObject.ToObject<FermentationStepDto>();
                case "sparge":
                    return jObject.ToObject<SpargeStepDto>();
                default:
                    return null;
            }
           
        }
 
        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}