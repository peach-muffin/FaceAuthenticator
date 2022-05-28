using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAuthenticator.Services
{
    public interface IBlobService
    {
        void upload(IFormFile formFile);
        public void upload(string filePath);

    }
}
