using System.Data.Common;

namespace Products.Domain.DataAccess.Repositories
{
    public interface IMySqlConnHelper
    {
        DbConnection MySqlConnection();
    }
}
