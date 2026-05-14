using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VendingFranchisee.Desktop.Helpers;
using VendingFranchisee.Desktop.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VendingFranchisee.Desktop.Models;

namespace VendingFranchisee.Desktop.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7202/api";

        public ApiService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }

        public void SetAuthToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<ApiUser?> LoginAsync(string email, string password)
        {
            try
            {
                var loginData = new { email, password };
                var json = JsonSerializer.Serialize(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/Auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(responseBody);
                    var token = doc.RootElement.GetProperty("token").GetString();

                    if (!string.IsNullOrEmpty(token))
                    {
                        var user = JwtHelper.DecodeToken(token);
                        SetAuthToken(user.Token);
                        return user;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<ApiVendingMachine>?> GetMachinesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/VendingMachines");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<ApiVendingMachine>>(json);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> AddMachineAsync(ApiVendingMachine machine)
        {
            try
            {
                var json = JsonSerializer.Serialize(machine);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/VendingMachines", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteMachineAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/VendingMachines/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}