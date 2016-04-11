using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    // [ElasticType(Name = "notification")]
    public class NotificationDto
    {
        [JsonProperty(PropertyName = "notificationId")]
        public int NotificationId { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "value")]
        public bool Value { get; set; }

    }
}
