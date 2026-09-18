using Microsoft.Data.SqlClient;

namespace Semana04NeptunoWpf.Services;

public static class DatabaseConnection
{
    // Se conserva la instancia LocalDB usada en el proyecto de Semana03.
    private const string ConnectionString =
        @"Server=(localdb)\MSSQLLocalDB;Database=Neptuno;Integrated Security=True;TrustServerCertificate=True;";

    public static SqlConnection GetConnection() => new(ConnectionString);
}
