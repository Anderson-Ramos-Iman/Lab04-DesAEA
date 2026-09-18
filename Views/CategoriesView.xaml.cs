using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using Semana04NeptunoWpf.Models;
using Semana04NeptunoWpf.Services;

namespace Semana04NeptunoWpf.Views;

public partial class CategoriesView : UserControl
{
    public CategoriesView()
    {
        InitializeComponent();
        LoadCategories();
    }

    private void LoadCategories()
    {
        try
        {
            var categories = new List<Category>();
            using SqlConnection connection = DatabaseConnection.GetConnection();
            using SqlCommand command = new("dbo.usp_ListarCategorias", connection) { CommandType = CommandType.StoredProcedure };
            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int activeOrdinal = reader.GetOrdinal("Activo");
                categories.Add(new Category
                {
                    IdCategoria = reader.GetInt32(reader.GetOrdinal("idcategoria")),
                    NombreCategoria = reader["nombrecategoria"] as string ?? string.Empty,
                    Descripcion = reader["descripcion"] as string ?? string.Empty,
                    Activo = reader.IsDBNull(activeOrdinal) ? null : reader.GetBoolean(activeOrdinal),
                    CodCategoria = reader["CodCategoria"] as string ?? string.Empty
                });
            }
            CategoriesDataGrid.ItemsSource = categories;
            StatusText.Text = $"{categories.Count} categoría(s) encontrada(s).";
        }
        catch (Exception ex)
        {
            StatusText.Text = "No se pudieron cargar las categorías.";
            MessageBox.Show(ex.Message, "Error de base de datos", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void RefreshButton_Click(object sender, RoutedEventArgs e) => LoadCategories();
}
