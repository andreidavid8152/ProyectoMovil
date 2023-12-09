namespace ProyectoApp;

public partial class MenuPage : ContentPage
{
    public MenuPage()
    {
        InitializeComponent();
        menuItemsCollectionView.ItemsSource = new List<MenuItem>
        {
            new MenuItem { Title = "Dashboard", TargetType = typeof(MainPage) },
            new MenuItem { Title = "Mis Locales", TargetType = typeof(MisLocalesPage) },
            new MenuItem { Title = "Mis Reservas", TargetType = typeof(ReservasPage) },
            new MenuItem { Title = "Mis Comentarios", TargetType = typeof(ComentariosPage) }
            
        };

        menuItemsCollectionView.SelectionChanged += OnSelectionChanged;
    }

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var collectionView = (CollectionView)sender;
        if (collectionView.SelectedItem is MenuItem item)
        {
            var page = (Page)Activator.CreateInstance(item.TargetType);
            var flyoutPage = this.Parent as FlyoutPage; // Obtiene la referencia del FlyoutPage actual
            if (flyoutPage != null)
            {
                flyoutPage.Detail = new NavigationPage(page)
                {
                    BarBackgroundColor = Color.FromHex("#14282f")
                };
                flyoutPage.IsPresented = false; // Esto cierra el menú después de la selección de un elemento
            }
            collectionView.SelectedItem = null; // Desmarca el elemento seleccionado
        }
    }

}

public class MenuItem
{
    public string Title { get; set; }
    public Type TargetType { get; set; }
}