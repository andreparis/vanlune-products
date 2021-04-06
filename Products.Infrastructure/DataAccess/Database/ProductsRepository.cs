using Dapper;
using Products.Domain.DataAccess.Repositories;
using Products.Domain.Entities;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Infrastructure.DataAccess.Database
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly IMySqlConnHelper _mySqlConnHelper;

        public ProductsRepository(IMySqlConnHelper mySqlConnHelper)
        {
            _mySqlConnHelper = mySqlConnHelper;
        }

        public async Task<int> InsertProduct(Product product)
        {
            var query = $@"INSERT INTO `Vanlune`.`Products`
                        (`title`,
                        `description`,
                        `sale`,
                        `price`,
                        `quantity`,
                        `discount`,
                        `images_src`,
                        `idCategory`)
                        VALUES
                        (@{nameof(Product.Title)},
                        @{nameof(Product.Description)},
                        @{nameof(Product.Sale)},
                        @{nameof(Product.Price)},
                        @{nameof(Product.Quantity)},
                        @{nameof(Product.Discount)},
                        @{nameof(Images.Src)},
                        @{nameof(Product.Category.Id)}); 
                        SELECT LAST_INSERT_ID();";

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var result = await connection.QueryAsync<int>(query, new 
            {
                product.Title,
                product.Description,
                product.Sale,
                product.Price,
                product.Quantity,
                product.Discount,
                product.Image.Src,
                product.Category.Id
            });

            return result.Single();
        }

        public async Task UpdateProduct(Product product)
        {
            var query = $@"UPDATE `Vanlune`.`Products`
                            SET
                            `title` =  @{nameof(Product.Title)},
                            `description` = @{nameof(Product.Description)},
                            `sale` = @{nameof(Product.Sale)},
                            `price` = @{nameof(Product.Price)},
                            `quantity` =  @{nameof(Product.Quantity)},
                            `discount` =@{nameof(Product.Discount)},
                            `images_src` = @{nameof(Images.Src)},
                            `idCategory` = @{nameof(Product.Category.Id)}
                            WHERE `id` = @idProduct;";

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var result = await connection.ExecuteAsync(query, new
            {
                product.Title,
                product.Description,
                product.Sale,
                product.Price,
                product.Quantity,
                product.Discount,
                product.Image.Src,
                product.Category.Id,
                idProduct = product.Id
            });
        }
    
        public async Task<int> DeleteProductById(int id)
        {
            var query = $@"DELETE `Vanlune`.`Products`
                            WHERE `id` = @idProduct;";

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var result = await connection.QueryAsync<int>(query, new
            {
                idProduct = id
            });

            return result.Single();
        }
    }
}
