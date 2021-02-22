using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Products.Domain.DataAccess.Repositories
{
    public interface IVariantsRepository
    {
        Task<int> InsertAsync(Variants variants);
        Task<IEnumerable<Variants>> GetAll();
    }
}
