using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using Semana04NeptunoWpf.Models;
using Semana04NeptunoWpf.Services;

namespace Semana04NeptunoWpf.Views;

public partial class ProvidersView : UserControl
{
    public ProvidersView()
    {
        InitializeComponent();
        LoadProviders();
    }

    private void LoadProviders()
    {
        try
        {
            using SqlConnection connection = DatabaseConnection.GetConnection();
            using SqlCommand command = new("dbo.usp_ListarProveedores", connection) { CommandType = CommandType.StoredProcedure };
            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            List<Provider> providers = ProviderReader.Read(reader);
            ProvidersDataGrid.ItemsSource = providers;
            StatusText.Text = $"{providers.Count} proveedor(es) encontrado(s).";
        }
        catch (Exception ex)
        {
            StatusText.Text = "No se pudieron cargar los proveedores.";
            MessageBox.Show(ex.Message, "Error de base de datos", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void RefreshButton_Click(object sender, RoutedEventArgs e) => LoadProviders();
}
