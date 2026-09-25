using System.Windows;
using System.Windows.Controls;
using Semana04NeptunoWpf.Views;

namespace Semana04NeptunoWpf;

public partial class MainWindow : Window
{
    public MainWindow() => InitializeComponent();

    private void ShowView(UserControl view, string title, string subtitle)
    {
        WelcomePanel.Visibility = Visibility.Collapsed;
        ContentArea.Content = view;
        PageTitle.Text = title;
        PageSubtitle.Text = subtitle;
    }

    private void ProductsButton_Click(object sender, RoutedEventArgs e) =>
        ShowView(new ProductsView(), "Productos", "Punto 3: listado mediante SqlDataReader.");

    private void CategoriesButton_Click(object sender, RoutedEventArgs e) =>
        ShowView(new CategoriesView(), "Categorías", "Punto 4: listado mediante SqlDataReader.");

    private void ProviderSearchButton_Click(object sender, RoutedEventArgs e) =>
        ShowView(new ProviderSearchView(), "Proveedores", "CRUD con procedimientos almacenados y SqlDataReader.");
}
