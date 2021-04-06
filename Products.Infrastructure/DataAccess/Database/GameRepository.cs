using Dapper;
using Products.Domain.DataAccess.Repositories;
using Products.Domain.Entities;
using Products.Infraestructure.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Products.Infrastructure.DataAccess.Database
{
    public class GameRepository : IGameRepository
    {
        private readonly IMySqlConnHelper _mySqlConnHelper;
        private readonly ILogger _logger;

        public GameRepository(IMySqlConnHelper mySqlConnHelper,
            ILogger logger)
        {
            _mySqlConnHelper = mySqlConnHelper;
            _logger = logger;
        }

        public async Task<IEnumerable<Game>> GetAll()
        {
            try
            {
                var query = $@"SELECT id,name FROM Vanlune.Games";

                using var connection = _mySqlConnHelper.MySqlConnection();

                if (connection.State != ConnectionState.Open)
                    connection.Open();

                var result = await connection.QueryAsync<Game>(query);

                return result;
            }
            catch (Exception e)
            {
                _logger.Error($"Error {e.Message} at {e.StackTrace}");

                return default;
            }
        }
    }
}
