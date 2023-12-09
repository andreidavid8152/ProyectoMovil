using ProyectoApp.Models;
using ProyectoApp.Services;
using System.Collections.ObjectModel;

namespace ProyectoApp;

public partial class MisLocalesPage : ContentPage
{
    private readonly APIService _api;
    public ObservableCollection<Local> Locales { get; set; }
    public MisLocalesPage()
    {
        InitializeComponent();
        _api = App.ServiceProvider.GetService<APIService>(); // Obtener la instancia de APIService del contenedor
        Locales = new ObservableCollection<Local>();
        this.BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarLocales();
    }


    private async void OnClickNuevoLocal(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NuevoLocalPage());
    }


    private async Task CargarLocales()
    {
        try
        {
            string token = Preferences.Get("UserToken", defaultValue: string.Empty);
            var locales = await _api.ObtenerLocalesArrendador(token);

            if (Locales.Count != locales.Count) // Comprueba si la cantidad de elementos es diferente
            {
                Locales.Clear();
                foreach (var local in locales)
                {
                    Locales.Add(local);
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudo obtener los locales: " + ex.Message, "OK");
        }
    }


    private void OnClickShowDetails(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Local local)
        {
            Navigation.PushAsync(new DetalleLocalPage(local.Id));
            // Restablece el �tem seleccionado para que no aparezca como seleccionado al regresar a la p�gina.
            listaLocales.SelectedItem = null;
        }
    }
}