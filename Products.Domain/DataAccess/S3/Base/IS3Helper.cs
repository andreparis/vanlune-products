using Amazon.S3.Model;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Products.Domain.DataAccess.S3.Base
{
    public interface IS3Helper<T> where T : class
    {
        IEnumerable<T> GetList(string keyName);
        T GetJsonFile(string keyName);
        IEnumerable<T> GetAll(string keyName);
        void Upload(string message, string keyName, string contentType = "application/json");
        Task<string> UploadFile(Stream inputStream, string fileName);
        void Delete(string keyName);
        ListObjectsV2Response ListObjectsInS3(string keyName);
        string GetAsString(string keyName);
    }
}
