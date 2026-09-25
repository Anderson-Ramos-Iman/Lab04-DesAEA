using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using Semana04NeptunoWpf.Services;

namespace Semana04NeptunoWpf.Views;

public partial class ProviderFormView : Window
{
    private readonly int? providerId;
    public ProviderFormView(int? id = null) { InitializeComponent(); providerId = id; if (id.HasValue) { TitleText.Text = "Editar proveedor"; Load(id.Value); } }
    private void Load(int id)
    {
        using SqlConnection c = DatabaseConnection.GetConnection(); using SqlCommand q = new("dbo.usp_Proveedores_ObtenerPorId", c) { CommandType = CommandType.StoredProcedure }; q.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = id; c.Open(); using SqlDataReader r = q.ExecuteReader();
        if (!r.Read()) return; CompanyBox.Text = Text(r,"nombreCompañia"); ContactBox.Text = Text(r,"nombrecontacto"); RoleBox.Text = Text(r,"cargocontacto"); AddressBox.Text = Text(r,"direccion"); CityBox.Text = Text(r,"ciudad"); RegionBox.Text = Text(r,"region"); PostalBox.Text = Text(r,"codPostal"); CountryBox.Text = Text(r,"pais"); PhoneBox.Text = Text(r,"telefono"); FaxBox.Text = Text(r,"fax"); WebBox.Text = Text(r,"paginaprincipal");
    }
    private static string Text(SqlDataReader r, string n) { int i=r.GetOrdinal(n); return r.IsDBNull(i) ? "" : r.GetValue(i).ToString() ?? ""; }
    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(CompanyBox.Text) || string.IsNullOrWhiteSpace(ContactBox.Text)) { MessageBox.Show("Nombre de compañía y contacto son obligatorios.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
        using SqlConnection c = DatabaseConnection.GetConnection(); string proc = providerId.HasValue ? "dbo.usp_Proveedores_Actualizar" : "dbo.usp_Proveedores_Insertar"; using SqlCommand q = new(proc, c) { CommandType = CommandType.StoredProcedure };
        if (providerId.HasValue) q.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = providerId.Value;
        Add(q,"@NombreCompania",40,CompanyBox.Text); Add(q,"@NombreContacto",30,ContactBox.Text); Add(q,"@CargoContacto",30,RoleBox.Text); Add(q,"@Direccion",60,AddressBox.Text); Add(q,"@Ciudad",15,CityBox.Text); Add(q,"@Region",15,RegionBox.Text); Add(q,"@CodPostal",10,PostalBox.Text); Add(q,"@Pais",15,CountryBox.Text); Add(q,"@Telefono",24,PhoneBox.Text); Add(q,"@Fax",24,FaxBox.Text); q.Parameters.Add("@PaginaPrincipal", SqlDbType.Text).Value = (object)WebBox.Text ?? DBNull.Value;
        c.Open(); q.ExecuteNonQuery(); MessageBox.Show("Proveedor guardado correctamente.", "Correcto", MessageBoxButton.OK, MessageBoxImage.Information); DialogResult = true;
    }
    private static void Add(SqlCommand q,string n,int size,string value) => q.Parameters.Add(n,SqlDbType.VarChar,size).Value = string.IsNullOrWhiteSpace(value) ? DBNull.Value : value.Trim();
    private void CancelButton_Click(object sender, RoutedEventArgs e) => Close();
}
