using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Products.Domain.DataAccess.Repositories;
using Products.Domain.DataAccess.Repositories.Base;
using Products.Infraestructure.Security;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Products.Infrastructure.DataAccess.Database.Base
{
    public class MySqlConnHelper : IMySqlConnHelper
    {
        private readonly string _connectionString;

        public MySqlConnHelper(IConfiguration configuration,
            IAwsSecretManagerService awsSecretManagerService)
        {
            var secret = JsonConvert.DeserializeObject<SecretDb>(awsSecretManagerService.GetSecret("db-dev"));
            _connectionString = $@"server={secret.Host};
                                userid={secret.Username};
                                password={secret.Password};
                                database=Vanlune;
                                Pooling=True;
                                Min Pool Size=0;
                                Max Pool Size=5;
                                Connection Lifetime=60; 
                                default command timeout=300;";
        }

        public DbConnection MySqlConnection()
        {
            return new MySql.Data.MySqlClient.MySqlConnection(_connectionString);
        }
    }
}
