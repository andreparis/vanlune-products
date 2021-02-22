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
    public class TagsRepository : ITagsRepository
    {
        private readonly IMySqlConnHelper _mySqlConnHelper;

        public TagsRepository(IMySqlConnHelper mySqlConnHelper)
        {
            _mySqlConnHelper = mySqlConnHelper;
        }

        public async Task<IEnumerable<Tags>> GetTags()
        {
            var query = $@"SELECT `Tags`.`id`,
                                `Tags`.`name`
                            FROM `Vanlune`.`Tags`";

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var result = await connection.QueryAsync<Tags>(query);

            return result;
        }

        public async Task<IEnumerable<Tags>> GetTagsByName(string[] names)
        {
            var query = $@"SELECT `Tags`.`id`,
                                `Tags`.`name`
                            FROM `Vanlune`.`Tags`
                            WHERE `Tags`.`name` IN @names";

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var result = await connection.QueryAsync<Tags>(query, new { names });

            return result;
        }

        public async Task<int> InsertAsync(Tags tags)
        {
            var query = $@"INSERT INTO `Vanlune`.`Tags`
                        (`name`)
                        VALUES
                        (@{nameof(tags.Name)});
                        SELECT LAST_INSERT_ID();";

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var result = await connection.QueryAsync<int>(query, new
            {
                tags.Name
            });

            return result.Single();
        }
    }
}
