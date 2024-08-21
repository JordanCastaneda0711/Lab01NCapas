using Entities.Models;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProxyServer
{
    public class CustomerProxy : ICustomerProxy
    {
        private readonly HttpClient _httpClient;

        public CustomerProxy()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7045/api/Customer/") // Asegúrate de que esta URL coincida con la configuración de tu servicio
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Customer> CreateAsync(Customer customer)
        {
            try
            {
                var json = JsonSerializer.Serialize(customer);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("", content);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Customer>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (global::System.Exception ex)
            {
                // Manejar la excepción (e.g., logging)
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }



        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{id}");
                return response.IsSuccessStatusCode;
            }
            catch (global::System.Exception ex)
            {
                // throw;
                // Manejar la exepcion (e.g., logging)
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }



        public async Task<List<Customer>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Customer>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            }
            catch (global::System.Exception ex)
            {
                //manejar la excepcion { extern.g., loggin}
                Console.WriteLine($"Error: {ex.Message}");
                return null;
                //throw
            }

        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            try
            {
                // Realiza una solicitud HTTP GET a una URL que incluye el ID del cliente
                var response = await _httpClient.GetAsync($"{id}");

                // Verifica si la solicitud fue exitosa (código de estado 200 OK)
                response.EnsureSuccessStatusCode();

                // Lee el contenido de la respuesta como una cadena JSON
                var json = await response.Content.ReadAsStringAsync();

                // Deserializa el JSON en un objeto de tipo Customer
                return JsonSerializer.Deserialize<Customer>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (global::System.Exception ex)
            {

                Console.WriteLine($"Error:   {ex.Message}");
                return null;
            }
        }
        public async Task<bool> UpdateAsync(int id, Customer customer)
        {
            try
            {
                var json = JsonSerializer.Serialize(customer);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{id}", content);

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Status Code: {response.StatusCode}, Response Content: {responseContent}");

                return response.IsSuccessStatusCode;
            }
            catch (global::System.Exception ex)
            {
                Console.WriteLine("Exception during PUT request: " + ex.Message);
                throw;
            }
        }

    }
}


