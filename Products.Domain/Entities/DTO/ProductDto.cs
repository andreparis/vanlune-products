using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Domain.Entities.DTO
{
    public class ProductDto : Product
    {
        public string ImageSrc { get; set; }
        [JsonProperty("variants")]
        public Variants[] Variants { get; set; }
        [JsonProperty("images")]
        public Images[] Images { get; set; }
        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        public object GroupBy(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }
}
