using Dapper;
using Products.Domain.DataAccess.Repositories;
using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Infrastructure.DataAccess.Database
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMySqlConnHelper _mySqlConnHelper;

        public CategoryRepository(IMySqlConnHelper mySqlConnHelper)
        {
            _mySqlConnHelper = mySqlConnHelper;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            var query = $@"SELECT `Category`.`id`,
                                `Category`.`name`,
                                `Category`.`description`
                            FROM `Vanlune`.`Category`;";

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var result = await connection.QueryAsync<Category>(query);

            return result;
        }

        public async Task<Category> GetCategory(int id)
        {
            var query = $@"SELECT `Category`.`id`,
                                `Category`.`name`,
                                `Category`.`description`
                            FROM `Vanlune`.`Category`;
                            WHERE
                            `Category`.`id` = @{nameof(id)}";

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var result = await connection.QueryAsync<Category>(query, new
            {
                id
            });

            return result.Single();
        }

        public async Task<int> InsertAsync(Category category)
        {
            var query = $@"INSERT INTO `Vanlune`.`Category`
                        (`name`,
                        `description`)
                        VALUES
                        (@{nameof(Category.Name)},
                        @{nameof(Category.Description)});
                        SELECT LAST_INSERT_ID();";

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var result = await connection.QueryAsync<int>(query, new
            {
                category.Name,
                category.Description
            });

            return result.Single();
        }
    
    
        public async Task<int> UpdateAsync(Category category)
        {
            var query = $@"UPDATE `Vanlune`.`Category`
                        SET
                        `name` =@{nameof(Category.Name)},
                        `description` = @{nameof(Category.Description)}>
                        WHERE `id` = @{nameof(Category.Id)};";

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var result = await connection.QueryAsync<int>(query, new
            {
                category.Name,
                category.Description,
                category.Id
            });

            return result.Single();
        }
    }
}
