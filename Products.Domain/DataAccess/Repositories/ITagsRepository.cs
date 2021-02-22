using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Products.Domain.DataAccess.Repositories
{
    public interface ITagsRepository
    {
        Task<IEnumerable<Tags>> GetTags();
        Task<IEnumerable<Tags>> GetTagsByName(string[] names);
        Task<int> InsertAsync(Tags tags);
    }
}
