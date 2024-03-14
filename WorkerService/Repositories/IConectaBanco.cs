
using System.Data.SqlClient;


namespace WorkerService.Repositories
{
    public interface IConectaBanco
    {
        SqlConnection getConection();
    }
}
