using Amazon.S3;
using Products.Domain.DataAccess.S3;
using Products.Domain.Entities;
using Products.Infraestructure.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Products.Infraestructure.DataAccess.S3
{
    public class ProductsS3 : S3Helper<string>, IProductsS3
    {
        private readonly string KEY_BASE = "products/";

        public ProductsS3(IConfiguration configuration, 
            ILogger logger) : base(logger, configuration) 
        {
        }

        public async Task<string> UploadImage(Stream inputStream, string fileName)
        {
            return await UploadFile(inputStream, string.Concat(KEY_BASE, fileName)).ConfigureAwait(false);
        }
    }
}
