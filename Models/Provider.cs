namespace Semana04NeptunoWpf.Models;

public sealed class Provider
{
    public int IdProveedor { get; init; }
    public string NombreCompania { get; init; } = string.Empty;
    public string NombreContacto { get; init; } = string.Empty;
    public string CargoContacto { get; init; } = string.Empty;
    public string Ciudad { get; init; } = string.Empty;
    public string Pais { get; init; } = string.Empty;
    public string Telefono { get; init; } = string.Empty;
}
