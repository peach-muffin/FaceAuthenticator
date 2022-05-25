using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAuthenticator.Model
{
    public class SignInRequest
    {
        [Required]
        public string UserName { get; set; }

    }

    public class SignInResponse
    {
        public string FirstName { get; set; }
        public string Guid { get; set; }
    }
}
