using Newtonsoft.Json;
using ProyectoApp.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace ProyectoApp.Services
{
    public class APIService
    {
        public static string _baseUrl;
        public HttpClient _httpClient;

        // Constructor: inicializa el URL base y el cliente HTTP.
        public APIService()
        {
            _baseUrl = "http://10.0.2.2:5260/api/";
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

        public async Task<bool> Registro(UserInput usuario)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Usuarios", usuario);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMessage);
            }
        }

        public async Task<String> Login(Login usuario)
        {

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Usuarios/loginApp", usuario);
            if (response.IsSuccessStatusCode)
            {
                // Deserializar el cuerpo de la respuesta para obtener el token
                var responseBody = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseBody);

                return tokenResponse.Token;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMessage);
            }

        }

        public async Task<UserInput> GetPerfil(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"{_baseUrl}Usuarios/perfil");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserInput>(content);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMessage);
            }
        }

        public async Task<bool> EditarPerfil(UserInput usuario, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(usuario), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}Usuarios/editarperfil", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMessage);
            }
        }

        public async Task<List<Local>> ObtenerTodosLocales(string token)
        {
            // Añade el token como header de autorización
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{_baseUrl}Locales");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var locales = JsonConvert.DeserializeObject<List<Local>>(content);
                return locales;
            }

            throw new Exception("No se pudo obtener los locales desde la API.");
        }

        public async Task<Local> ObtenerLocal(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{_baseUrl}Locales/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var local = JsonConvert.DeserializeObject<Local>(content);
                return local;
            }

            throw new Exception("No se pudo obtener los locales desde la API.");
        }

        public async Task<bool> Reservar(Reserva reserva, string token)
        {
            // Añade el token como header de autorización
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Reservas", reserva);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMessage);
            }

        }

        public async Task<List<Reserva>> ObtenerReservasCliente(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{_baseUrl}Reservas/Cliente");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var reserva = JsonConvert.DeserializeObject<List<Reserva>>(content);
                return reserva;
            }

            throw new Exception("No se pudo obtener las reservas desde la API.");

        }

        public async Task<bool> EliminarReserva(int id, string token)
        {
            // Añade el token como header de autorización
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Realiza la petición HTTP DELETE
            var response = await _httpClient.DeleteAsync($"{_baseUrl}Reservas/{id}");

            // Verifica si la petición fue exitosa
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMessage);
            }
        }

        public async Task<bool> ComentarReserva(Comentario comentario, string token)
        {
            // Añade el token como header de autorización
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Realiza la petición HTTP POST
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Reservas/comentar", comentario);

            // Verifica si la petición fue exitosa
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMessage);
            }
        }

        public async Task<List<Comentario>> ObtenerComentariosPorUsuario(int usuarioId, string token)
        {
            // Añade el token como header de autorización
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Realiza la petición HTTP GET
            var response = await _httpClient.GetAsync($"{_baseUrl}Comentarios/{usuarioId}");

            // Verifica si la petición fue exitosa
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var comentarios = JsonConvert.DeserializeObject<List<Comentario>>(content);
                return comentarios;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMessage);
            }
        }

        public async Task<bool> EliminarComentario(int comentarioId, string token)
        {
            // Añade el token como header de autorización
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Realiza la petición HTTP DELETE
            var response = await _httpClient.DeleteAsync($"{_baseUrl}Comentarios/{comentarioId}");

            // Verifica si la petición fue exitosa
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMessage);
            }
        }

        public async Task<List<Local>> ObtenerLocalesArrendador(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{_baseUrl}Locales/Arrendador");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var localesArrendador = JsonConvert.DeserializeObject<List<Local>>(content);
                return localesArrendador;
            }

            throw new Exception("No se pudo obtener los locales del arrendador desde la API.");
        }
    }
}
