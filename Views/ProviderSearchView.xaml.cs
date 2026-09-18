using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using Semana04NeptunoWpf.Models;
using Semana04NeptunoWpf.Services;

namespace Semana04NeptunoWpf.Views;

public partial class ProviderSearchView : UserControl
{
    public ProviderSearchView() => InitializeComponent();

    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            using SqlConnection connection = DatabaseConnection.GetConnection();
            using SqlCommand command = new("dbo.usp_BuscarProveedoresPorContactoYCiudad", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.Add("@NombreContacto", SqlDbType.VarChar, 30).Value = ContactTextBox.Text.Trim();
            command.Parameters.Add("@Ciudad", SqlDbType.VarChar, 15).Value = CityTextBox.Text.Trim();
            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            List<Provider> providers = ProviderReader.Read(reader);
            ProvidersDataGrid.ItemsSource = providers;
            StatusText.Text = $"{providers.Count} proveedor(es) encontrado(s).";
        }
        catch (Exception ex)
        {
            StatusText.Text = "No se pudo realizar la búsqueda.";
            MessageBox.Show(ex.Message, "Error de base de datos", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
