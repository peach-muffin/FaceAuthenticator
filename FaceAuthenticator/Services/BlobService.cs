using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
namespace FaceAuthenticator.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IConfiguration _configuration;

        public BlobService(BlobServiceClient blobServiceClient, IConfiguration configuration)
        {
            _blobServiceClient = blobServiceClient;
            _configuration = configuration;
        }
        public void upload(IFormFile formFile)
        {
            var containerName = _configuration.GetSection("Storage:ContainerName").Value;

            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(formFile.FileName);

            using var stream = formFile.OpenReadStream();
            blobClient.Upload(stream, true);

        }
        public void upload(string filePath)
        {
            var containerName = _configuration.GetSection("Storage:ContainerName").Value;

            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            string filename = Path.GetFileName(filePath);
            var blobClient = containerClient.GetBlobClient(filename);

            blobClient.Upload(filePath, true);

        }


    }
}
