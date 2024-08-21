using Entities.Models;
using ProxyServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProxyServer
{
    public class ProductProxy : IProductProxy
    {
        private readonly HttpClient _httpClient;

        public ProductProxy()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7045/api/Product/") // Asegúrate de que esta URL coincida con la configuración de tu servicio
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Product> CreateAsync(Product product)
        {
            try
            {
                // Serializar el objeto product a JSON
                var json = JsonSerializer.Serialize(product);

                // Crear el contenido de la solicitud con el JSON
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Enviar la solicitud POST al servidor
                var response = await _httpClient.PostAsync("", content);

                // Leer y registrar el contenido de la respuesta
                var responseJson = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Status Code: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseJson}");

                // Asegurarse de que la respuesta fue exitosa
                response.EnsureSuccessStatusCode();

                // Deserializar el contenido de la respuesta a un objeto Product
                return JsonSerializer.Deserialize<Product>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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
                // Manejar la excepción (e.g., logging)
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Product>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (global::System.Exception ex)
            {
                // Manejar la excepción (e.g., logging)
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{id}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Product>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (global::System.Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateAsync(int id, Product product)
        {
            try
            {
                var json = JsonSerializer.Serialize(product);
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