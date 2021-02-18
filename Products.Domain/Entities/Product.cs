using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Domain.Entities
{
    public class Product : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public bool Sale { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public Tag[] Tags { get; set; }
    }
}
