namespace Semana04NeptunoWpf.Models;

public sealed class Category
{
    public int IdCategoria { get; init; }
    public string NombreCategoria { get; init; } = string.Empty;
    public string Descripcion { get; init; } = string.Empty;
    public bool? Activo { get; init; }
    public string CodCategoria { get; init; } = string.Empty;
}
