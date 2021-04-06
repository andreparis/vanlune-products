using Dapper;
using Newtonsoft.Json;
using Products.Domain.DataAccess.Repositories;
using Products.Domain.Entities;
using Products.Infrastructure.DataAccess.Database.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Infrastructure.DataAccess.Database
{
    public class CustomizeRepository : ICustomizeRepository
    {
        private readonly IMySqlConnHelper _mySqlConnHelper;

        public CustomizeRepository(IMySqlConnHelper mySqlConnHelper)
        {
            _mySqlConnHelper = mySqlConnHelper;

            SqlMapper.AddTypeHandler(new DapperCustomizeTypeHandler());
        }

        public async Task<List<int>> AddAllAsync(IEnumerable<Customize> customizes)
        {
            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();
            var ids = new List<int>();
            var transaction = connection.BeginTransaction();
            try
            {
                foreach(var customize in customizes)
                {
                    var query = $@"INSERT INTO `Vanlune`.`Customize`
                        (name,
                        value,
                        idGame)
                        VALUES
                        (@{nameof(Customize.Name)},
                        @{nameof(Customize.Value)},
                        @{nameof(Customize.Game.Id)}); 
                        SELECT LAST_INSERT_ID();";

                    var result = await connection.QueryAsync<int>(query, new
                    {
                        customize.Name,
                        value = JsonConvert.SerializeObject(customize.Value),
                        customize.Game.Id
                    });
                    if (result != null && result.First() > 0)
                        ids.Add(result.First());
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }

            return ids;
        }

        public async Task<int> AddAsync(Customize customize)
        {
            var query = $@"INSERT INTO `Vanlune`.`Customize`
                        (name,
                        value,
                        idGame)
                        VALUES
                        (@{nameof(Customize.Name)},
                        @{nameof(Customize.Value)},
                        @{nameof(Customize.Game.Id)}); 
                        SELECT LAST_INSERT_ID();";

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var result = await connection.QueryAsync<int>(query, new
            {
                customize.Name,
                value = JsonConvert.SerializeObject(customize.Value),
                customize.Game.Id
            });

            return result.Single();
        }

        public async Task UpdateAllAsync(IEnumerable<Customize> customizes)
        {
            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var transaction = connection.BeginTransaction();
            try
            {
                foreach (var customize in customizes)
                {
                    var query = $@"UPDATE `Vanlune`.`Customize`
                        SET name = @{nameof(Customize.Name)},
                        value = @{nameof(Customize.Value)}
                        WHERE id = @{nameof(Customize.Id)}";

                    var result = await connection.QueryAsync<int>(query, new
                    {
                        customize.Name,
                        value = JsonConvert.SerializeObject(customize.Value),
                        customize.Id
                    });
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
        }

        public async Task<IEnumerable<Customize>> GetCustomizesByIds(int[] ids)
        {
            var query = new StringBuilder();
            query.Append($@"SELECT `Customize`.`id`,
                            `Customize`.`name`,
                            `Customize`.`Value`
                        FROM `Vanlune`.`Customize`
                        WHERE id in @ids");

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var result = await connection.QueryAsync<Customize>(query.ToString(), new { ids });

            return result;
        }

        public async Task<IEnumerable<Customize>> GetCustomizesByFilters(IDictionary<string, string> filters)
        {
            var query = new StringBuilder();
            query.Append($@"SELECT 
                            C.`id`           as {nameof(Customize.Id)},
                            C.`name`         as {nameof(Customize.Name)},
                            C.`Value`        as {nameof(Customize.Value)},
                            G.id             as {nameof(Game.Id)},
                            G.name           as {nameof(Game.Name)}
                        FROM `Vanlune`.`Customize` AS C
                        LEFT JOIN `Vanlune`.Games AS  G ON C.idGame = G.id
                        WHERE ");
            var param = new DynamicParameters();
            var hasId = filters.ContainsKey("id") && !string.IsNullOrEmpty(filters["id"]);
            if (hasId)
            {
                query.Append($" C.`id`=@{nameof(Customize.Id)} ");
                param.Add(nameof(Customize.Id), filters["id"]);
            }
            
            var hasName = filters.ContainsKey("name") && !string.IsNullOrEmpty(filters["name"]);
            if (hasName)
            {
                if (hasId) query.Append(" AND ");
                query.Append($" C.`name`=@{nameof(Customize.Name)} ");
                param.Add(nameof(Customize.Name), filters["name"]);
            }
            
            var hasValue = filters.ContainsKey("value") && !string.IsNullOrEmpty(filters["value"]);
            if (hasValue)
            {
                if (hasId || hasName) query.Append(" AND ");
                query.Append($" C.`value`=@{nameof(Customize.Value)} ");
                param.Add(nameof(Customize.Value), filters["value"]);
            }

            var hasGame = filters.ContainsKey("game") && !string.IsNullOrEmpty(filters["game"]);
            if (hasGame)
            {
                if (hasId || hasName || hasValue) query.Append(" AND ");
                query.Append($" C.`idGame`= @game ");
                param.Add("game", filters["game"]);
            }

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var result = await connection.QueryAsync<Customize, Game, Customize>(query.ToString(), 
                (customize, game) => 
                {
                    customize.Game = game;

                    return customize;
                },
                param,
                splitOn: $"{nameof(Customize.Id)},{nameof(Game.Id)}");

            return result;
        }
    
        public async Task<IEnumerable<Customize>> GetCustomizeLinkedToProduct(int[] ids)
        {
            var query = new StringBuilder();
            query.Append($@"SELECT 
                            C.`id`,
                            C.`name`,
                            C.`Value`
                        FROM `Vanlune`.`ProductCustomize` AS P
                        JOIN `Vanlune`.Customize AS C ON P.idCustomize = C.id
                        WHERE 
                        P.idCustomize in @ids");

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var result = await connection.QueryAsync<Customize>(query.ToString(), new { ids });

            return result;
        }

        public async Task DeleteAllByIdAsync(int[] ids)
        {
            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var transaction = connection.BeginTransaction();
            try
            {
                foreach (var id in ids)
                {
                    var query = $@"DELETE FROM 
                        `Vanlune`.`Customize`
                        WHERE id = @id";

                    var result = await connection.QueryAsync<int>(query, new
                    {
                        id
                    });
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
