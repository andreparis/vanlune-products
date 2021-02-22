using Dapper;
using Products.Domain.DataAccess.Repositories;
using Products.Domain.Entities;
using Products.Domain.Entities.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Products.Infrastructure.DataAccess.Database.Aggregation
{
    public class ProductAggregationRepository : IProductAggregationRepository
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IVariantsRepository _variantsRepository;
        private readonly ITagsRepository _tagsRepository;
        private readonly IMySqlConnHelper _mySqlConnHelper;

        public ProductAggregationRepository(ICategoryRepository categoryRepository,
            IProductsRepository productsRepository,
            IVariantsRepository variantsRepository,
            ITagsRepository tagsRepository,
            IMySqlConnHelper mySqlConnHelper)
        {
            _categoryRepository = categoryRepository;
            _productsRepository = productsRepository;
            _variantsRepository = variantsRepository;
            _tagsRepository = tagsRepository;
            _mySqlConnHelper = mySqlConnHelper;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsDtoByCategory(string categoryName)
        {
            var query = $@"SELECT 
                        P.`id`          AS {nameof(ProductDto.Id)},
                        P.`title`       AS {nameof(ProductDto.Title)},
                        P.`description` AS {nameof(ProductDto.Description)},
                        P.`sale`        AS {nameof(ProductDto.Sale)},
                        P.`price`       AS {nameof(ProductDto.Price)},
                        P.`quantity`    AS {nameof(ProductDto.Quantity)},
                        P.`discount`    AS {nameof(ProductDto.Discount)},
                        P.`images_src`  AS {nameof(Images.Src)},
                        P.`idCategory`  AS {nameof(Category.Id)},
                        C.id            AS {nameof(Category.Id)},
                        C.name          AS {nameof(Category.Name)},
                        V.id            AS {nameof(Variants.Id)},
                        V.name          AS {nameof(Variants.Name)},
                        V.factor        AS {nameof(Variants.Factor)},
                        T.id            AS {nameof(Tags.Id)},
                        T.name          AS {nameof(Tags.Name)}
                        FROM Vanlune.Products as P
                        JOIN Vanlune.Category as C on P.idCategory = C.id
                        LEFT JOIN Vanlune.ProductsVariants as PV on P.id = PV.idProduct
                        LEFT JOIN Vanlune.Variants as V on PV.idVariant = V.id
                        LEFT JOIN Vanlune.ProductsTags as PT on P.id = PT.idProducts
                        LEFT JOIN Vanlune.Tags as T on PT.idTags = T.id
                        WHERE C.Name = @categoryName;";

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            var result = await connection.QueryAsync<ProductDto, Images, Category, Variants, Tags, ProductDto>(query,
            (prductDto, image, category, variant, tags) =>
            {
                if (image != null)
                    prductDto.Image = new Images() { Src = image.Src };
                if (category != null)
                    prductDto.Category = category;
                if (variant != null)
                    prductDto.Variants = new Variants[] { variant };
                if (tags != null && !string.IsNullOrEmpty(tags.Name))
                    prductDto.Tags = new string[] { tags.Name };

                return prductDto;
            }, new
            {
                categoryName
            }, splitOn: $"{nameof(Images.Src)},{nameof(Category.Id)},{nameof(Variants.Id)},{nameof(Tags.Id)}") ;

            return result;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsTagsByCategory(int categoryId)
        {
            var query = $@"SELECT 
                        P.`id`          AS {nameof(ProductDto.Id)},
                        T.id            AS {nameof(Tags.Id)},
                        T.name          AS {nameof(Tags.Name)}
                        FROM Vanlune.Products as P
                        JOIN Vanlune.ProductsTags as PT on P.id = PT.idProducts
                        JOIN Vanlune.Tags as T on PT.idTags = T.id
                        WHERE P.idCategory = @categoryId;";

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            var result = await connection.QueryAsync<ProductDto, Images, Category, Variants, ProductDto>(query,
            (prductDto, image, category, variant) =>
            {
                if (image != null)
                    prductDto.Image = new Images() { Src = image.Src };
                if (category != null)
                    prductDto.Category = category;
                if (variant != null)
                    prductDto.Variants = new Variants[] { variant };

                return prductDto;
            }, new
            {
                categoryId
            }, splitOn: $"{nameof(Images.Src)},{nameof(Category.Id)},{nameof(Variants.Id)},{nameof(Tags.Id)}");

            return result;
        }

        public async Task InsertNewProduct(ProductDto product)
        {
            var productId = await _productsRepository.InsertProduct(product).ConfigureAwait(false);
            product.Id = productId;

            if (product.Variants.Length > 0) 
                await SetVariantsToProduct(product.Variants, product);
            if (product.Tags.Length > 0)
                await SetTagsToProduct(product.Tags, product);
        }

        public async Task UpdateProduct(Product product)
        {
            await _productsRepository.UpdateProduct(product).ConfigureAwait(false);
        }

        public async Task DeleteProduct(ProductDto product)
        {
            await RemoveVariantsByProducts(product.Variants, product).ConfigureAwait(false);
            await RemoveTagsByProduct(product.Tags, product).ConfigureAwait(false);
            await _productsRepository.DeleteProductById(product.Id).ConfigureAwait(false);
        }

        public async Task SetVariantsToProduct(IEnumerable<Variants> variants, Product product)
        {
            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            using var transaction = connection.BeginTransaction();
            try
            {
                foreach (var variant in variants)
                {
                    var query = $@"INSERT INTO Vanlune.ProductsVariants 
                        (idProduct, idVariant)
                        SELECT * FROM 
                        (SELECT @idProduct,@idVariant) AS tmp
                        WHERE NOT EXISTS (
                            SELECT idProduct,idVariant  
                            FROM Vanlune.ProductsVariants 
                            WHERE idProduct=@idProduct 
                            AND idVariant=@idVariant
                        );";

                    await connection.ExecuteAsync(query, new 
                    {
                        idProduct = product.Id,
                        idVariant = variant.Id
                    }).ConfigureAwait(false);
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
        }
    
        public async Task RemoveVariantsByProducts(IEnumerable<Variants> variants, Product product)
        {
            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            using var transaction = connection.BeginTransaction();
            try
            {
                foreach (var variant in variants)
                {
                    var query = $@"DELETE FROM Vanlune.ProductsVariants 
                                WHERE idProduct = @{nameof(Product.Id)} 
                                AND idVariant = @idVariant;";

                    await connection.ExecuteAsync(query, new
                    {
                        product.Id,
                        idVariant = variant.Id
                    }).ConfigureAwait(false);
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
        }

        public async Task SetTagsToProduct(string[] tagsNames, Product product)
        {
            var tags = await _tagsRepository.GetTagsByName(tagsNames).ConfigureAwait(false);

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            using var transaction = connection.BeginTransaction();
            try
            {
                foreach (var tag in tags)
                {
                    var query = $@"INSERT INTO Vanlune.ProductsTags 
                        (idProducts, idTags)
                        SELECT * FROM 
                        (SELECT @idProducts,@idTags) AS tmp
                        WHERE NOT EXISTS (
                            SELECT idProducts,idTags  
                            FROM Vanlune.ProductsTags 
                            WHERE idProducts=@idProducts 
                            AND idTags=@idTags
                        );";

                    await connection.ExecuteAsync(query, new
                    {
                        idProducts = product.Id,
                        idTags = tag.Id
                    }).ConfigureAwait(false);
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }

        }

        public async Task RemoveTagsByProduct(string[] tagsNames, Product product)
        {
            var tags = await _tagsRepository.GetTagsByName(tagsNames).ConfigureAwait(false);

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            using var transaction = connection.BeginTransaction();
            try
            {
                foreach (var tag in tags)
                {
                    var query = $@"DELETE FROM Vanlune.ProductsTags 
                                WHERE idProducts = @{nameof(Product.Id)} 
                                AND idTags = @idTags;";

                    await connection.ExecuteAsync(query, new
                    {
                        product.Id,
                        idTags = tag.Id
                    }).ConfigureAwait(false);
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
        }
    }
}
