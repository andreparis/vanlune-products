using Products.Domain.Entities;
using Products.Domain.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Products.Domain.DataAccess.Repositories
{
    public interface IProductAggregationRepository
    {
        Task<IEnumerable<ProductDto>> GetProductsDtoByFilters(IDictionary<string, string> filters);
        Task InsertNewProduct(ProductDto product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(ProductDto product);
        Task SetVariantsToProduct(IEnumerable<Variants> variants, Product product);
        Task RemoveVariantsByProducts(IEnumerable<Variants> variants, Product product);
        Task SetTagsToProduct(string[] tagsNames, Product product);
        Task RemoveTagsByProduct(string[] tagsNames, Product product);
        Task SetCustomizesToProduct(IEnumerable<Customize> customizes, Product product);
        Task RemoveCustomizesByProduct(IEnumerable<Customize> customizes, Product product);
    }
}
