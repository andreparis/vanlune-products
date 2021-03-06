﻿using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Products.Domain.DataAccess.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategoriesByGameId(int idGame);
        Task<IEnumerable<Category>> GetAll();
        Task<Category> GetCategory(int id);
        Task<int> InsertAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
    }
}
