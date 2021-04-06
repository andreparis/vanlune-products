using Newtonsoft.Json;

namespace Products.Domain.Entities
{
    public class Category : Entity
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("game")]
        public Game Game { get; set; }
        [JsonProperty("imageSrc")]
        public string ImageSrc { get; set; }
    }
}
