using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using Mapa.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Mapa.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _client;


        public S3Service(IAmazonS3 client)
        {
            _client = client;
        }

        public async Task<S3Response> CreateBucketAsync(string bucketName)
        {
            try
            {
                if (await AmazonS3Util.DoesS3BucketExistAsync(_client, bucketName) == false)
                {
                    var putBucketRequest = new PutBucketRequest
                    {
                        BucketName = bucketName,
                        UseClientRegion = true
                    };
                    var response = await _client.PutBucketAsync(putBucketRequest);

                    return new S3Response
                    {
                        Message = response.ResponseMetadata.RequestId,
                        Status = response.HttpStatusCode
                    };
                }

            }
            catch (AmazonS3Exception e)
            {
                return new S3Response
                {
                    Message = e.Message,
                    Status = e.StatusCode
                };
            }
            catch (Exception e)
            {
                return new S3Response
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = e.Message
                };
            }

            return new S3Response
            {
                Status = HttpStatusCode.InternalServerError,
                Message = "Algo deu errado!"
            };

        }

        public async Task<S3Response> DeleteBucketAsync(string bucketName)
        {
            try
            {
                if (await AmazonS3Util.DoesS3BucketExistAsync(_client, bucketName) == true)
                {
                    var deleteBucketRequest = new DeleteBucketRequest
                    {
                        BucketName = bucketName,
                        UseClientRegion = true
                    };

                    var response = await _client.DeleteBucketAsync(deleteBucketRequest);

                    return new S3Response
                    {
                        Message = response.ResponseMetadata.RequestId,
                        Status = response.HttpStatusCode
                    };

                }
            }
            catch (AmazonS3Exception e)
            {
                return new S3Response
                {
                    Message = e.Message,
                    Status = e.StatusCode
                };
            }
            catch (Exception e)
            {
                return new S3Response
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = e.Message
                };
            }

            return new S3Response
            {
                Status = HttpStatusCode.InternalServerError,
                Message = "Bucket não existe"
            };
        }

        public async Task<S3Response> DoesBucketOrFolderExist(string path)
        {
            try
            {
                if (await AmazonS3Util.DoesS3BucketExistAsync(_client, path) == true)
                {
                    return new S3Response
                    {
                        Status = HttpStatusCode.InternalServerError,
                        Message = "Existe"
                    };
                }
                else
                {
                    return new S3Response
                    {
                        Status = HttpStatusCode.InternalServerError,
                        Message = "Não existe"
                    };
                }
            }
            catch (AmazonS3Exception e)
            {
                return new S3Response
                {
                    Message = e.Message,
                    Status = e.StatusCode
                };
            }
            catch (Exception e)
            {
                return new S3Response
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = e.Message
                };
            }
        }

        public async Task<S3Response> UploadFile(MemoryStream stream, string path, string bucket)
        {
            try
            {
                using (Amazon.S3.Transfer.TransferUtility transferUti = new Amazon.S3.Transfer.TransferUtility(_client))
                {
                    transferUti.Upload(stream, bucket, path);
                    return new S3Response
                    {
                        Status = HttpStatusCode.OK,
                        Message = "Enviado com sucesso."
                    };
                }
            }
            catch (AmazonS3Exception e)
            {
                return new S3Response
                {
                    Message = e.Message,
                    Status = e.StatusCode
                };
            }
            catch (Exception e)
            {
                return new S3Response
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = e.Message
                };
            }
        }

        public async Task<S3Response> CopyObjectAsync(string pathOrigem, string pathDestino, string bucketOrigem, string bucketDestino)
        {
            try
            {
                using (Amazon.S3.Transfer.TransferUtility transferUti = new Amazon.S3.Transfer.TransferUtility(_client))
                {
                    CopyObjectRequest request = new CopyObjectRequest
                    {
                        SourceBucket = bucketOrigem,
                        SourceKey = pathOrigem,
                        DestinationBucket = bucketDestino,
                        DestinationKey = pathDestino
                    };
                    await _client.CopyObjectAsync(request);
                    return new S3Response
                    {
                        Status = HttpStatusCode.OK,
                        Message = "Enviado com sucesso."
                    };
                }
            }
            catch (AmazonS3Exception e)
            {
                return new S3Response
                {
                    Message = e.Message,
                    Status = e.StatusCode
                };
            }
            catch (Exception e)
            {
                return new S3Response
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = e.Message
                };
            }
        }

        public async Task<S3Response> DeleteFile(string bucketName, string pathAmazon)
        {
            try
            {
                using (Amazon.S3.Transfer.TransferUtility transferUti = new Amazon.S3.Transfer.TransferUtility(_client))
                {
                    var deleteObjectRequest = new DeleteObjectRequest
                    {
                        BucketName = bucketName,
                        Key = pathAmazon
                    };

                    await _client.DeleteObjectAsync(deleteObjectRequest);
                    return new S3Response
                    {
                        Status = HttpStatusCode.OK,
                        Message = "Enviado com sucesso."
                    };
                }
            }
            catch (AmazonS3Exception e)
            {
                return new S3Response
                {
                    Message = e.Message,
                    Status = e.StatusCode
                };
            }
            catch (Exception e)
            {
                return new S3Response
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = e.Message
                };
            }
        }

    }
}
