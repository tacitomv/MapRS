using Mapa.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mapa.Services
{
    public interface IS3Service
    {
        Task<S3Response> DoesBucketOrFolderExist(string path);
        Task<S3Response> CreateBucketAsync(string bucketName);
        Task<S3Response> DeleteBucketAsync(string bucketName);
        Task<S3Response> UploadFile(MemoryStream stream, string path, string bucketName);
        Task<S3Response> CopyObjectAsync(string pathOrigem, string pathDestino, string bucketOrigem, string bucketDestino);
        Task<S3Response> DeleteFile(string bucketName, string pathAmazon);
    }
}
