using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using WpfApp1.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WpfApp1.Api
{
    public class ExamApiClient
    {
        private HttpClient _httpClient = new HttpClient();
        public ExamApiClient()
        {
            _httpClient.BaseAddress = new Uri("https://localhost:7071/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Exam>> GetAllExamsAsync()
        {
            var response = await _httpClient.GetAsync("api/Exam");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<Exam>>();

            return result;
        }

        public async Task<Exam> GetExamAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/Exam/{id}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<Exam>();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task UpdateExamAsync(int id, Exam exam)
        {
            try
            {
                var json = JsonSerializer.Serialize(exam);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"api/Exam/{id}", content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<Exam> CreateExamAsync(Exam exam)
        {
            try
            {
                var json = JsonSerializer.Serialize(exam);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/Exam", content);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<Exam>();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task DeleteExamAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Exam/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
