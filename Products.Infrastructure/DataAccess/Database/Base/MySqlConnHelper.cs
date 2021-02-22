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
            //var secret = JsonConvert.DeserializeObject<SecretDb>(awsSecretManagerService.GetSecret(configuration["DB_SECRET"]));
            //server=collateral-dev1;userid=LCOP_ANALISTA;password=LCOP_ANALISTA;database=cop_position;Pooling=True;Min Pool Size=0;Max Pool Size=5;Connection Lifetime=60; default command timeout=300;
            _connectionString = @"server=vanlune-db-dev.cgt0b7bfafur.us-east-1.rds.amazonaws.com;
                                userid=usr_vanlune_dev;
                                password=FJL6ftJdha6jmh!;
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
