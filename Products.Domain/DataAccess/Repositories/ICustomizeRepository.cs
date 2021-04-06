using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Products.Domain.DataAccess.Repositories
{
    public interface ICustomizeRepository
    {
        Task<List<int>> AddAllAsync(IEnumerable<Customize> customizes);
        Task<int> AddAsync(Customize customize);
        Task UpdateAllAsync(IEnumerable<Customize> customizes);
        Task<IEnumerable<Customize>> GetCustomizesByFilters(IDictionary<string, string> filters);
        Task<IEnumerable<Customize>> GetCustomizesByIds(int[] ids);
        Task<IEnumerable<Customize>> GetCustomizeLinkedToProduct(int[] ids);
        Task DeleteAllByIdAsync(int[] ids);
    }
}
