using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Domain.Entities
{
    public class Entity
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
