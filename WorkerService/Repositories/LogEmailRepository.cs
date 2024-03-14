using System.Data.SqlClient;
using WorkerService.Entities;

namespace WorkerService.Repositories
{
    public class LogEmailRepository : ILogEmailRepository
    {
        private readonly IConectaBanco _conectaBanco;

        public LogEmailRepository(IConectaBanco conectaBanco)
        {
            _conectaBanco = conectaBanco;
        }

        public void Inserir(LogEmail logEmail)
        {

            SqlConnection connection = _conectaBanco.getConection();

            try
            {
                connection.Open();
                var sql = @"INSERT INTO LogEmail(data,para,conteudo) VALUES(@data,@para,@conteudo)";

                SqlCommand myCommand = new SqlCommand(sql, connection);
                myCommand.Parameters.AddWithValue("@data", DateTime.Now);
                myCommand.Parameters.AddWithValue("@para", logEmail.Para);
                myCommand.Parameters.AddWithValue("@conteudo", logEmail.Conteudo);

                myCommand.ExecuteNonQuery();
            }    
            finally
            {
                connection.Close();
            }

        }
    }
}
