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
    public class VariantsRepository : IVariantsRepository
    {
        private readonly IMySqlConnHelper _mySqlConnHelper;

        public VariantsRepository(IMySqlConnHelper mySqlConnHelper)
        {
            _mySqlConnHelper = mySqlConnHelper;
        }

        public async Task<int> InsertAsync(Variants variants)
        {
            var query = $@"INSERT INTO `Vanlune`.`Variants`
                        (`name`,
                        `factor`)
                        VALUES
                        (@{nameof(Variants.Name)},
                        @{nameof(Variants.Factor)});
                        SELECT LAST_INSERT_ID();";

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var result = await connection.QueryAsync<int>(query, new
            {
                variants.Name,
                variants.Factor
            });

            return result.Single();
        }
    
        public async Task<IEnumerable<Variants>> GetAll()
        {
            var query = $@"SELECT `Variants`.`id`,
                            `Variants`.`name`,
                            `Variants`.`factor`
                        FROM `Vanlune`.`Variants`;";

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var result = await connection.QueryAsync<Variants>(query);

            return result;
        }
    }
}
