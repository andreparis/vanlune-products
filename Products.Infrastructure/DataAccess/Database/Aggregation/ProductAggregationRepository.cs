using Dapper;
using Products.Domain.DataAccess.Repositories;
using Products.Domain.Entities;
using Products.Domain.Entities.DTO;
using Products.Infrastructure.DataAccess.Database.Extensions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Infrastructure.DataAccess.Database.Aggregation
{
    public class ProductAggregationRepository : IProductAggregationRepository
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IVariantsRepository _variantsRepository;
        private readonly ITagsRepository _tagsRepository;
        private readonly ICustomizeRepository _customizeRepository;
        private readonly IMySqlConnHelper _mySqlConnHelper;

        private readonly string SelectQueryBase = $@"SELECT 
                        P.`id`          AS {nameof(ProductDto.Id)},
                        P.`title`       AS {nameof(ProductDto.Title)},
                        P.`description` AS {nameof(ProductDto.Description)},
                        P.`sale`        AS {nameof(ProductDto.Sale)},
                        P.`price`       AS {nameof(ProductDto.Price)},
                        P.`quantity`    AS {nameof(ProductDto.Quantity)},
                        P.`discount`    AS {nameof(ProductDto.Discount)},
                        P.`images_src`  AS {nameof(ProductDto.ImageSrc)},
                        G.`id`          AS {nameof(Game.Id)},
                        G.`Name`        AS {nameof(Game.Name)},
                        C.id            AS {nameof(Category.Id)},
                        C.name          AS {nameof(Category.Name)},
                        V.id            AS {nameof(Variants.Id)},
                        V.name          AS {nameof(Variants.Name)},
                        V.factor        AS {nameof(Variants.Factor)},
                        V.idServer      AS {nameof(Server.Id)},
                        T.id            AS {nameof(Tags.Id)},
                        T.name          AS {nameof(Tags.Name)},
                        CM.id           AS {nameof(Customize.Id)},
                        CM.name         AS {nameof(Customize.Name)},
                        CM.value        AS {nameof(Customize.Value)}
                        FROM Vanlune.Products as P
                        LEFT JOIN Vanlune.Category as C on P.idCategory = C.id
                        LEFT JOIN Vanlune.ProductsVariants as PV on P.id = PV.idProduct
                        LEFT JOIN Vanlune.Variants as V on PV.idVariant = V.id
                        LEFT JOIN Vanlune.ProductsTags as PT on P.id = PT.idProducts
                        LEFT JOIN Vanlune.Tags as T on PT.idTags = T.id
                        LEFT JOIN Vanlune.Games as G on G.id = C.idGame
                        LEFT JOIN Vanlune.ProductCustomize as PC on PC.idProduct = P.id
                        LEFT JOIN Vanlune.Customize as CM on CM.id = PC.idCustomize";

        public ProductAggregationRepository(ICategoryRepository categoryRepository,
            IProductsRepository productsRepository,
            IVariantsRepository variantsRepository,
            ITagsRepository tagsRepository,
            ICustomizeRepository customizeRepository,
            IMySqlConnHelper mySqlConnHelper)
        {
            _categoryRepository = categoryRepository;
            _productsRepository = productsRepository;
            _variantsRepository = variantsRepository;
            _tagsRepository = tagsRepository;
            _customizeRepository = customizeRepository;
            _mySqlConnHelper = mySqlConnHelper;

            SqlMapper.AddTypeHandler(new DapperCustomizeTypeHandler());
        }

        public async Task<IEnumerable<ProductDto>> GetProductsDtoByFilters(IDictionary<string, string> filters)
        {
            var query = new StringBuilder();
            query.Append(SelectQueryBase);
            query.Append(" WHERE ");

            var inTerms = new DynamicParameters();

            var hasId = filters.ContainsKey("id") && !string.IsNullOrEmpty(filters["id"]);
            if (hasId)
            {
                query.Append(" P.`id`=@id ");
                inTerms.Add("@id", filters["id"]);
            }

            var hasCategory = filters.ContainsKey("category") && !string.IsNullOrEmpty(filters["category"]);
            if (hasCategory)
            {
                if (hasId) query.Append(" AND ");
                query.Append(" P.`idCategory`=@category ");
                inTerms.Add("@category", filters["category"]);
            }

            var hasCategoryName = filters.ContainsKey("categoryName") && !string.IsNullOrEmpty(filters["categoryName"]);
            if (hasCategoryName)
            {
                if (hasId || hasCategory) query.Append(" AND ");
                query.Append(" C.name=@categoryName ");
                inTerms.Add("@categoryName", filters["categoryName"]);
            }

            var hasGame = filters.ContainsKey("game") && !string.IsNullOrEmpty(filters["game"]);
            if (hasGame)
            {
                if (hasId || hasCategory || hasCategoryName) query.Append(" AND ");
                query.Append(" C.`idGame`=@game ");
                inTerms.Add("@game", filters["game"]);
            }

            var hasTitle = filters.ContainsKey("title") && !string.IsNullOrEmpty(filters["title"]);
            if (hasTitle)
            {
                if (hasId || hasCategory || hasCategoryName || hasGame) query.Append(" AND ");
                query.Append(" P.`title`=@title ");
                inTerms.Add("@title", filters["title"]);
            }

            var hasTag = filters.ContainsKey("tagName") && !string.IsNullOrEmpty(filters["tagName"]);
            if (hasTag)
            {
                if (hasId || hasCategory || hasCategoryName || hasGame || hasTitle) query.Append(" AND ");
                query.Append(" T.Name = @tagName ");
                inTerms.Add("@tagName", filters["tagName"]);
            }

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            var result = await connection
            .QueryAsync<ProductDto, Game, Category, Variants, Server, Tags, Customize, ProductDto>
            (query.ToString(),
            (prductDto, game, category, variant, server, tags, customize) =>
            {
                prductDto.Category = category;

                if (category != null)
                    prductDto.Category.Game = game;
                if (variant != null)
                    variant.Server = server;
                if (!string.IsNullOrEmpty(prductDto.ImageSrc))
                    prductDto.Image = new Images() { Src = prductDto.ImageSrc };
                if (variant != null)
                    prductDto.Variants = new Variants[] { variant };
                if (tags != null && !string.IsNullOrEmpty(tags.Name))
                    prductDto.Tags = new string[] { tags.Name };
                if (customize != null)
                    prductDto.Customizes = new List<Customize>() { customize };

                return prductDto;
            }, inTerms,
            splitOn: $"{nameof(Game.Id)},{nameof(Category.Id)},{nameof(Variants.Id)},{nameof(Tags.Id)},{nameof(Server.Id)},{nameof(Customize.Id)}");

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
            if (product.Customizes.Count() > 0)
                await SetCustomizesToProduct(product.Customizes, product);

        }

        public async Task UpdateProduct(Product product)
        {
            await _productsRepository.UpdateProduct(product).ConfigureAwait(false);
        }

        public async Task DeleteProduct(ProductDto product)
        {
            await RemoveVariantsByProducts(product.Variants, product).ConfigureAwait(false);
            await RemoveTagsByProduct(product.Tags, product).ConfigureAwait(false);
            await RemoveCustomizesByProduct(product.Customizes, product).ConfigureAwait(false);

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
                        SELECT  idProduct, 
                                idVariant FROM 
                        (SELECT @idProduct as idProduct,@idVariant as idVariant) AS tmp
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
            catch (Exception e)
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
                        SELECT idProducts, idTags FROM 
                        (SELECT @idProducts as idProducts,
                                @idTags as idTags) AS tmp
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

        public async Task SetCustomizesToProduct(IEnumerable<Customize> customizes, Product product)
        {
            var result = await _customizeRepository
                .GetCustomizesByIds(customizes.Select(a => a.Id).ToArray())
                .ConfigureAwait(false);

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            using var transaction = connection.BeginTransaction();
            try
            {
                foreach (var customize in result)
                {
                    var query = $@"INSERT INTO Vanlune.ProductCustomize 
                        (idProduct, idCustomize)
                        SELECT idProduct, idCustomize FROM 
                        (SELECT @idProduct as idProduct,
                                @idCustomize as idCustomize) AS tmp
                        WHERE NOT EXISTS (
                            SELECT idProduct,idCustomize  
                            FROM Vanlune.ProductCustomize 
                            WHERE idProduct=@idProduct 
                            AND idCustomize=@idCustomize
                        );";

                    await connection.ExecuteAsync(query, new
                    {
                        idProduct = product.Id,
                        idCustomize = customize.Id
                    }).ConfigureAwait(false);
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }

        }

        public async Task RemoveCustomizesByProduct(IEnumerable<Customize> customizes, Product product)
        {
            var result = await _customizeRepository
                .GetCustomizesByIds(customizes.Select(a => a.Id).ToArray())
                .ConfigureAwait(false);

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            using var transaction = connection.BeginTransaction();
            try
            {
                foreach (var customize in result)
                {
                    var query = $@"DELETE FROM Vanlune.ProductCustomize 
                                WHERE idProduct = @idProduct
                                AND idCustomize = @idCustomize;";

                    await connection.ExecuteAsync(query, new
                    {
                        idProduct = product.Id,
                        idCustomize = customize.Id
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
