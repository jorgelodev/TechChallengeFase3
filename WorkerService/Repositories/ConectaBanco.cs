using System.Data.SqlClient;

namespace WorkerService.Repositories
{
    public class ConectaBanco : IConectaBanco
    {
        private readonly IConfiguration _configuration;
        public ConectaBanco(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlConnection getConection()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            return con;
        }
    }
}
