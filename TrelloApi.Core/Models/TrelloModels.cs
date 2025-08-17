using Newtonsoft.Json;
namespace TrelloApi.Core.Models
{
    public class Board
    {
        [JsonProperty("id")] public string Id { get; set; } = string.Empty;
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
    }
    public class List
    {
        [JsonProperty("id")] public string Id { get; set; } = string.Empty;
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("idBoard")]
        public string BoardId { get; set; } = string.Empty;
    }
    public class Card
    {
        [JsonProperty("id")] public string Id { get; set; } = string.Empty;
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("idList")]
        public string ListId { get; set; } = string.Empty;
    }
}