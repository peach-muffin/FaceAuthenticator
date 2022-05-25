using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FaceAuthenticator.Model;

namespace FaceAuthenticator.DataAccessLayer
{
    public interface IAuthDL
    {
        public Task<SignUpResponse> SignUp(SignUpRequest request);
        public Task<SignInResponse> SignIn(SignInRequest request);

    }
}
