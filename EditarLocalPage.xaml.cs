using ProyectoApp.Models;
using ProyectoApp.Services;

namespace ProyectoApp;

public partial class EditarLocalPage : ContentPage
{
    private readonly APIService _api;
    private int _localId;
    public EditarLocalPage(int localId)
    {
        InitializeComponent();
        _api = App.ServiceProvider.GetService<APIService>(); // Obtener la instancia de APIService del contenedor
        _localId = localId;
        CargarDatosLocal();
    }

    private async void CargarDatosLocal()
    {
        try
        {
            string token = Preferences.Get("UserToken", defaultValue: string.Empty);
            var local = await _api.ObtenerLocal(_localId, token);
            // Asigna los datos a los controles
            NombreEntry.Text = local.Nombre;
            DescripcionEditor.Text = local.Descripcion;
            DireccionEntry.Text = local.Direccion;
            CapacidadEntry.Text = local.Capacidad.ToString();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudo cargar los datos del local: " + ex.Message, "OK");
        }
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        var local = new Local
        {
            Nombre = NombreEntry.Text,
            Descripcion = DescripcionEditor.Text,
            Direccion = DireccionEntry.Text,
            Capacidad = int.Parse(CapacidadEntry.Text) // Asegúrate de manejar la conversión y validación correctamente
        };

        // Aquí puedes llamar a tu API para guardar los cambios
    }

    private void OnVolverClicked(object sender, EventArgs e)
    {
        // Código para volver a la página anterior
    }
}