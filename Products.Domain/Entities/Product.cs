using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Domain.Entities
{
    public class Product : Entity
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("category")]
        public Category Category { get; set; }
        [JsonProperty("sale")]
        public bool Sale { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
        [JsonProperty("discount")]
        public decimal Discount { get; set; }
        [JsonProperty("image")]
        public Images Image { get; set; }
        [JsonProperty("customizes")]
        public IEnumerable<Customize> Customizes { get; set; }
    }
}
