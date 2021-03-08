using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Products.Domain.DataAccess.Repositories
{
    public interface IServerRepository
    {
        Task AddServer(Server server);
        Task<IEnumerable<Server>> GetAll(int gameId = 1);
    }
}
