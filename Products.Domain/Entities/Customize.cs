using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Products.Domain.Entities
{
    public class Customize : Entity
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public IList<CustomizeValue> Value { get; set; }
        [JsonProperty("game")]
        public Game Game { get; set; }

        public static IList<Customize> RemoveDuplicates(IEnumerable<Customize> customizes)
        {
            var newCustomizes = new List<Customize>();

            foreach (var c in customizes)
            {
                if (c!=null && !newCustomizes.Any(a => a.Name.Equals(c.Name)))
                    newCustomizes.Add(c);
            }

            return newCustomizes;
        }
    }

    public class CustomizeValue
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("factor")]
        public decimal Factor { get; set; }
    }
}
