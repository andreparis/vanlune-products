using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Products.Domain.Rest
{
    public interface ISecretApi
    {
        Task<string> GetSecretAsync(string secret);
    }
}
