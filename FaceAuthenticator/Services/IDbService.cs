using MySql.Data.MySqlClient;

namespace FaceAuthenticator.Services
{
    public interface IDbService
    {
        MySqlConnection MySqlConnection { get; set; }
    }
}