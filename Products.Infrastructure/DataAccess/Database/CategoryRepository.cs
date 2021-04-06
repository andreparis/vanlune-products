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

        public async Task<IEnumerable<Category>> GetCategoriesByGameId(int idGame)
        {
            var query = $@"SELECT 
                                C.`id`            AS {nameof(Category.Id)},
                                C.`name`          AS {nameof(Category.Name)},
                                C.`description`   AS {nameof(Category.Description)},
                                C.`imageSrc`      AS {nameof(Category.ImageSrc)},
                                G.id              AS {nameof(Game.Id)},
                                G.name            AS {nameof(Game.Name)}
                            FROM `Vanlune`.`Category` AS C
                            LEFT JOIN `Vanlune`.Games AS G on C.idGame = G.id
                            WHERE G.id=@idGame;";

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var result = await connection.QueryAsync<Category, Game, Category>(query,
                (category, game) =>
                {
                    category.Game = game;

                    return category;
                }, new { idGame },
                splitOn: $"{nameof(Category.Id)},{nameof(Game.Id)}");

            return result;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            var query = $@"SELECT 
                                C.`id`            AS {nameof(Category.Id)},
                                C.`name`          AS {nameof(Category.Name)},
                                C.`description`   AS {nameof(Category.Description)},
                                C.`imageSrc`      AS {nameof(Category.ImageSrc)},
                                G.id              AS {nameof(Game.Id)},
                                G.name            AS {nameof(Game.Name)}
                            FROM `Vanlune`.`Category` AS C
                            LEFT JOIN Games AS G on C.idGame = G.id;";

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var result = await connection.QueryAsync<Category, Game, Category>(query,
                (category, game) => 
                {
                    category.Game = game;

                    return category;
                },
                splitOn: $"{nameof(Category.Id)},{nameof(Game.Id)}");

            return result;
        }

        public async Task<Category> GetCategory(int id)
        {
            var query = $@"SELECT `Category`.`id`,
                                `Category`.`name`,
                                `Category`.`description`
                            FROM `Vanlune`.`Category`
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
                        `description`,
                        `idGame`)
                        VALUES
                        (@{nameof(Category.Name)},
                        @{nameof(Category.Description)},
                        @{nameof(Category.Game.Id)});
                        SELECT LAST_INSERT_ID();";

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var result = await connection.QueryAsync<int>(query, new
            {
                category.Name,
                category.Description,
                category.Game.Id
            });

            return result.Single();
        }
        
        public async Task UpdateAsync(Category category)
        {
            var query = $@"UPDATE `Vanlune`.`Category`
                        SET
                        `name` =@{nameof(Category.Name)},
                        `description` = @{nameof(Category.Description)}
                        WHERE `id` = @{nameof(Category.Id)};";

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var result = await connection.ExecuteAsync(query, new
            {
                category.Name,
                category.Description,
                category.Id
            });
        }

        public async Task DeleteAsync(int id)
        {
            var query = $@"DELETE FROM `Vanlune`.`Category`
                        WHERE `id` = @id;";

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var result = await connection.ExecuteAsync(query, new
            {
                id
            });
        }
    }
}
