using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAuthenticator.Services
{
    public class DbService: IDbService
    { 
        private readonly IConfiguration _configuration;
        public  MySqlConnection MySqlConnection { get; set; }
       
        public DbService(IConfiguration configuration)
        {
            _configuration = configuration;
            MySqlConnection = new MySqlConnection(_configuration["ConnectionStrings:MySqlDBConnection"]);
            //MySqlConnection.Open();
        }
    }
}
