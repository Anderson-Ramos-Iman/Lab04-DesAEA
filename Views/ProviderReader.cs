using Microsoft.Data.SqlClient;
using Semana04NeptunoWpf.Models;

namespace Semana04NeptunoWpf.Views;

internal static class ProviderReader
{
    // Convierte fila por fila el SqlDataReader a objetos; no se usa DataTable.
    internal static List<Provider> Read(SqlDataReader reader)
    {
        var providers = new List<Provider>();
        while (reader.Read())
        {
            providers.Add(new Provider
            {
                IdProveedor = reader.GetInt32(reader.GetOrdinal("idProveedor")),
                NombreCompania = Text(reader, "nombreCompañia"),
                NombreContacto = Text(reader, "nombrecontacto"),
                CargoContacto = Text(reader, "cargocontacto"),
                Ciudad = Text(reader, "ciudad"),
                Pais = Text(reader, "pais"),
                Telefono = Text(reader, "telefono"), Fax = Text(reader, "fax"), Direccion = Text(reader, "direccion"),
                Region = Text(reader, "region"), CodPostal = Text(reader, "codPostal"), PaginaPrincipal = Text(reader, "paginaprincipal"),
                Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
            });
        }
        return providers;
    }

    private static string Text(SqlDataReader reader, string column)
    {
        int ordinal = reader.GetOrdinal(column);
        return reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);
    }
}
