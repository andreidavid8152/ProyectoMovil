using ProyectoApp.Models;
using ProyectoApp.Services;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Windows.Input;

namespace ProyectoApp;

public partial class ComentariosPage : ContentPage
{
    public ICommand EliminarComentarioCommand { get; }
    private readonly APIService _api;
    public ObservableCollection<Comentario> Comentarios { get; set; }
    public ComentariosPage()
    {
        InitializeComponent();
        _api = App.ServiceProvider.GetService<APIService>();
        Comentarios = new ObservableCollection<Comentario>();
        this.BindingContext = this;
        EliminarComentarioCommand = new Command<Comentario>(async (comentario) =>
        {
            await EliminarComentario(comentario);
        });
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarComentarios();
    }

    private async Task CargarComentarios()
    {
        try
        {
            string token = Preferences.Get("UserToken", string.Empty);
            // Suponiendo que puedes obtener el ID del usuario actual de alguna manera
            int usuarioId = GetUserIdFromToken(token);

            var comentariosObtenidos = await _api.ObtenerComentariosPorUsuario(usuarioId, token);

            Comentarios.Clear();
            foreach (var comentario in comentariosObtenidos)
            {
                Comentarios.Add(comentario);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudieron cargar los comentarios: " + ex.Message, "OK");
        }
    }

    public async Task EliminarComentario(Comentario comentarioAEliminar)
    {
        // Confirmar con el usuario si realmente desea eliminar la reserva
        bool confirm = await DisplayAlert("Eliminar Comentario", "¿Estás seguro de que quieres eliminar este comentario?", "Sí", "No");
        if (!confirm)
        {
            return; // El usuario canceló la operación
        }
        try
        {
            string token = Preferences.Get("UserToken", string.Empty);
            await _api.EliminarComentario(comentarioAEliminar.Id, token);
            Comentarios.Remove(comentarioAEliminar);
            // Mensaje de confirmación
            await DisplayAlert("Comentario Eliminado", "El comentario ha sido eliminado correctamente.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudo eliminar el comentario: " + ex.Message, "OK");
        }
    }



    private int GetUserIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
        var userIdClaim = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;
        return int.Parse(userIdClaim);
    }
}