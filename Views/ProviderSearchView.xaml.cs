using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using Semana04NeptunoWpf.Models;
using Semana04NeptunoWpf.Services;

namespace Semana04NeptunoWpf.Views;

public partial class ProviderSearchView : UserControl
{
    public ProviderSearchView() { InitializeComponent(); LoadProviders(); }
    private void LoadProviders()
    {
        try
        {
            using SqlConnection connection = DatabaseConnection.GetConnection();
            using SqlCommand command = new("dbo.usp_Proveedores_Buscar", connection) { CommandType = CommandType.StoredProcedure };
            command.Parameters.Add("@NombreContacto", SqlDbType.VarChar, 30).Value = ContactTextBox.Text.Trim();
            command.Parameters.Add("@Ciudad", SqlDbType.VarChar, 15).Value = CityTextBox.Text.Trim();
            connection.Open(); using SqlDataReader reader = command.ExecuteReader();
            List<Provider> providers = ProviderReader.Read(reader); ProvidersDataGrid.ItemsSource = providers;
            StatusText.Text = $"{providers.Count} proveedor(es) encontrado(s).";
        }
        catch (Exception ex) { StatusText.Text = "No se pudo cargar la lista."; MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    private void SearchButton_Click(object sender, RoutedEventArgs e) => LoadProviders();
    private void CriteriaTextChanged(object sender, TextChangedEventArgs e) { if (IsLoaded) LoadProviders(); }
    private void NewButton_Click(object sender, RoutedEventArgs e) { new ProviderFormView().ShowDialog(); LoadProviders(); }
    private void EditButton_Click(object sender, RoutedEventArgs e) { if (sender is Button b && b.DataContext is Provider p) { new ProviderFormView(p.IdProveedor).ShowDialog(); LoadProviders(); } }
    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button b || b.DataContext is not Provider p) return;
        if (MessageBox.Show($"¿Eliminar lógicamente a {p.NombreCompania}?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
        using SqlConnection connection = DatabaseConnection.GetConnection(); using SqlCommand command = new("dbo.usp_Proveedores_Eliminar", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = p.IdProveedor; connection.Open(); command.ExecuteNonQuery();
        MessageBox.Show("Proveedor eliminado.", "Correcto", MessageBoxButton.OK, MessageBoxImage.Information); LoadProviders();
    }
}
