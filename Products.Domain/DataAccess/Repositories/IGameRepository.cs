using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Products.Domain.DataAccess.Repositories
{
    public interface IGameRepository
    {
        Task<IEnumerable<Game>> GetAll();
    }
}
