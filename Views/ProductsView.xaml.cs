using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using Semana04NeptunoWpf.Models;
using Semana04NeptunoWpf.Services;

namespace Semana04NeptunoWpf.Views;

public partial class ProductsView : UserControl
{
    public ProductsView()
    {
        InitializeComponent();
        LoadProducts();
    }

    private void LoadProducts()
    {
        try
        {
            var products = new List<Product>();
            using SqlConnection connection = DatabaseConnection.GetConnection();
            using SqlCommand command = new("dbo.usp_ListarProductos", connection) { CommandType = CommandType.StoredProcedure };
            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int priceOrdinal = reader.GetOrdinal("precioUnidad");
                int stockOrdinal = reader.GetOrdinal("unidadesEnExistencia");
                products.Add(new Product
                {
                    IdProducto = reader.GetInt32(reader.GetOrdinal("idproducto")),
                    NombreProducto = reader["nombreProducto"] as string ?? string.Empty,
                    PrecioUnidad = reader.IsDBNull(priceOrdinal) ? null : reader.GetDecimal(priceOrdinal),
                    UnidadesEnExistencia = reader.IsDBNull(stockOrdinal) ? null : reader.GetInt16(stockOrdinal),
                    CategoriaProducto = reader["categoriaProducto"] as string ?? string.Empty
                });
            }
            ProductsDataGrid.ItemsSource = products;
            StatusText.Text = $"{products.Count} producto(s) encontrado(s).";
        }
        catch (Exception ex)
        {
            StatusText.Text = "No se pudieron cargar los productos.";
            MessageBox.Show(ex.Message, "Error de base de datos", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void RefreshButton_Click(object sender, RoutedEventArgs e) => LoadProducts();
}
