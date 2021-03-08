using Dapper;
using Products.Domain.DataAccess.Repositories;
using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Products.Infrastructure.DataAccess.Database
{
    public class ServerRepository : IServerRepository
    {
        private readonly IMySqlConnHelper _mySqlConnHelper;

        public ServerRepository(IMySqlConnHelper mySqlConnHelper)
        {
            _mySqlConnHelper = mySqlConnHelper;
        }

        public async Task AddServer(Server server)
        {
            var query = $@"INSERT INTO `Vanlune`.`Servers`
                            (`Name`)
                            VALUES
                            (@{nameof(Server.Name)});
                            ";

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            await connection.ExecuteAsync(query, new { server.Name });
        }

        public async Task<IEnumerable<Server>> GetAll(int gameId = 1)
        {
            var query = $@"SELECT 
                            S.`id`   AS {nameof(Server.Id)},
                            S.`Name` AS {nameof(Server.Name)}
                            FROM `Vanlune`.`Servers` AS S
                            JOIN `Vanlune`.GamesServer AS G ON S.id = G.idServer
                            WHERE G.idGames = @gameId;";

            using var connection = _mySqlConnHelper.MySqlConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            var result = await connection.QueryAsync<Server>(query, new { gameId });

            return result;

        }
    }
}
