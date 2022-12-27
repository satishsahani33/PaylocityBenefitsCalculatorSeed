using Microsoft.Data.SqlClient;
using System.Data;

namespace Api.DataContext
{
    public class DapperApplicationContext
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;
        public DapperApplicationContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlDbConnection");
        }
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
