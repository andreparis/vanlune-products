using Products.Domain.DataAccess.S3.Base;
using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Products.Domain.DataAccess.S3
{
    public interface IProductsS3 : IS3Helper<string>
    {
        Task<string> UploadImage(Stream inputStream, string fileName);
    }
}
