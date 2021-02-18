using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Infraestructure.Security
{
    public interface IAwsSecretManagerService
    {
        string GetSecret(string secret);
    }
}
