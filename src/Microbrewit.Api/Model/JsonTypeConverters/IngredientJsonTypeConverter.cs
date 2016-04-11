using System;
using System.Collections.Generic;
using Microbrewit.Api.Model.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microbrewit.Api.Model.JsonTypeConverters
{
    public class IngredientJsonTypeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer,value);
        }
 
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var ingredients = new List<IIngredientStepDto>();
            if (reader.TokenType != JsonToken.StartArray) return ingredients;
            var jsonArray = JArray.Load(reader);
            foreach (var jObject in jsonArray.Children())
            {
                var ingredient = GetConcreteType(jObject);
                if(ingredient != null)
                    ingredients.Add(ingredient);
            }
            return ingredients;
        }
 
        private IIngredientStepDto GetConcreteType(JToken jObject)
        {
            if (!jObject.HasValues) return null;
            var type = jObject["type"];
            if (type == null) return null;
            switch (type.ToString())
            {
                case "hop":
                    return jObject.ToObject<HopStepDto>();
                case "fermentable":
                    return jObject.ToObject<FermentableStepDto>();
                case "yeast":
                    return jObject.ToObject<YeastStepDto>();
                case "other":
                    return jObject.ToObject<OtherStepDto>();    
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