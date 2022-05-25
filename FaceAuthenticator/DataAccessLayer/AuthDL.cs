using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using FaceAuthenticator.Model;
using FaceAuthenticator.Services;

namespace FaceAuthenticator.DataAccessLayer
{
    public class AuthDL : IAuthDL
    {
        private readonly IConfiguration _configuration;
        private readonly MySqlConnection _mySqlConnection;
        private IDbService _dbService; 


        public AuthDL(IConfiguration configuration, IDbService dbService)
        {
            _configuration = configuration;
            _mySqlConnection = dbService.MySqlConnection;
            _dbService = dbService;
        }

        //public async Task<SignInResponse> InitializeDb()
        //{

        //    try
        //    {
        //        if (_mySqlConnection.State != System.Data.ConnectionState.Open)
        //        {
        //            await _mySqlConnection.OpenAsync();
        //        }

        //        string SqlQuery = @"select FirstName, Guid from crudoperation.userdetail where UserName=@UserName";

        //        using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
        //        {
        //            sqlCommand.CommandType = System.Data.CommandType.Text;
        //            sqlCommand.CommandTimeout = 180;


        //            int status = await sqlCommand.ExecuteNonQueryAsync();

        //            if(status <= 0)
        //            {
                        
        //            }
                   
        //        }

        //    }
        //    finally
        //    {
        //        await _mySqlConnection.CloseAsync();
        //        await _mySqlConnection.DisposeAsync();
        //    }

        //    return response;
        //}


        public async Task<SignInResponse> SignIn(SignInRequest request)
        {
            SignInResponse response = new SignInResponse();
            
            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string SqlQuery = @"select FirstName, Guid from crudoperation.userdetail where UserName=@UserName";

                using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;
                    sqlCommand.Parameters.AddWithValue("@UserName", request.UserName);

                    using(DbDataReader dataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                      
                        while (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                response.FirstName = dataReader.GetString(0); ;
                                response.Guid = dataReader.GetString(1); ;
                            }

                            dataReader.NextResult();
                        }
                    }
                }

            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<SignUpResponse> SignUp(SignUpRequest request)
        {
            SignUpResponse response = new SignUpResponse();
            response.IsSuccess = true;
            response.Message = "Successful";
            try
            {

                if (!request.Password.Equals(request.ConfirmPassword))
                {
                    response.IsSuccess = false;
                    response.Message = "Password and confirm password did not match.";
                    return response;
                }

                if(_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string SqlQuery = @"insert into crudoperation.userdetail (FirstName, LastName, UserName, Email, Password, Guid, FaceId) values (@FirstName, @LastName, @UserName, @Email, @Password, @Guid, @FaceId)";
                request.Guid = Guid.NewGuid();
                using(MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;
                    sqlCommand.Parameters.AddWithValue("@FirstName", request.FirstName);
                    sqlCommand.Parameters.AddWithValue("@LastName", request.LastName);
                    sqlCommand.Parameters.AddWithValue("@UserName", request.UserName);
                    sqlCommand.Parameters.AddWithValue("@Email", request.Email);
                    sqlCommand.Parameters.AddWithValue("@Password", request.Password);
                    sqlCommand.Parameters.AddWithValue("@Guid", request.Guid);
                    sqlCommand.Parameters.AddWithValue("@FaceId", null);
                    int Status = await sqlCommand.ExecuteNonQueryAsync();
                    if (Status <= 0)
                    {
                        response.IsSuccess = false;
                        response.Message = "Something went wrong. Try Again!!";
                        return response;
                    }
                }

            }catch(Exception e)
            {
                response.IsSuccess = false;
                response.Message = e.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }

            return response;
        }
    }
}
