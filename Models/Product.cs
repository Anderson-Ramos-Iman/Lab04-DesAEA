namespace Semana04NeptunoWpf.Models;

public sealed class Product
{
    public int IdProducto { get; init; }
    public string NombreProducto { get; init; } = string.Empty;
    public decimal? PrecioUnidad { get; init; }
    public short? UnidadesEnExistencia { get; init; }
    public string CategoriaProducto { get; init; } = string.Empty;
}
