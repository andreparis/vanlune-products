using Products.Domain.DataAccess.S3.Base;
using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Domain.DataAccess.S3
{
    public interface IProductsRepository : IS3Helper<Product>
    {
        void DeleteProducts(string setting);
        void AddProduct(string settingName, Product setting);
        IEnumerable<Product> GetAllProduct();
        Product GetProduct(string product);
        IEnumerable<Product> GetProductsByCategory(string category);
    }
}
