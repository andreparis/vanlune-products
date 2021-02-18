using Amazon.S3;
using Products.Domain.DataAccess.S3;
using Products.Domain.Entities;
using Products.Infraestructure.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Infraestructure.DataAccess.S3
{
    public class ProductsRepository : S3Helper<Product>, IProductsRepository
    {
        private readonly string KEY_BASE = "product/";

        public ProductsRepository(IConfiguration configuration, 
            ILogger logger) : base(logger, configuration) 
        {
        }

        public void DeleteProducts(string product)
        {
            var key = string.Concat(KEY_BASE, product);

            Delete(key);
        }

        public void AddProduct(string path, Product product)
        {
            var key = string.Concat(KEY_BASE, path);

            Upload(JsonConvert.SerializeObject(product), key);
        }

        public IEnumerable<Product> GetAllProduct()
        {
            return GetAll(KEY_BASE);
        }

        public Product GetProduct(string product)
        {
            var key = string.Concat(KEY_BASE, product);

            return GetJsonFile(key);
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            var key = string.Concat(KEY_BASE, category, "/");

            return GetAll(key);
        }
    }
}
