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
            // Set the base address of the API
            _httpClient.BaseAddress = new Uri("https://localhost:7071/");
            // Clear the accept header and add a new one for JSON data
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // Get all exams from the API
        public async Task<List<Exam>> GetAllExamsAsync()
        {
            var response = await _httpClient.GetAsync("api/Exam");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<Exam>>();

            return result;
        }

        // Get a specific exam by name from the API
        public async Task<Exam> GetExamAsyncByName(string name)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/Exam/{name}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<Exam>();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        // Update an exam in the API
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

        // Create a new exam in the API
        public async Task<Exam> CreateExamAsync(Exam exam)
        {
            HttpResponseMessage response = null;
            try
            {
                var json = JsonSerializer.Serialize(exam);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                response = await _httpClient.PostAsync("api/Exam", content);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<Exam>();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\nAn exam with the same name already exists");
    }
        }

        // Delete an exam from the API
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

        // Get all exam results from the API
        public async Task<List<ExamResult>> GetAllExamResultsAsync()
        {
            var response = await _httpClient.GetAsync("api/ExamResult");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<ExamResult>>();

            return result;
        }

        // Get a specific exam result by ID and exam ID from the API
        public async Task<ExamResult> GetExamResultAsync(int id, int examId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/ExamResult/{id}/{examId}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<ExamResult>();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        // Get all exam results for a specific
        public async Task<List<ExamResult>> GetExamStaticAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/ExamResult/Static/{id}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<List<ExamResult>>();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        // Update an exam results for a specific
        public async Task UpdateExamResultAsync(int id, ExamResult examResult)
        {
            try
            {
                var json = JsonSerializer.Serialize(examResult);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"api/ExamResult/{id}", content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        // Create a new exam results in the API
        public async Task<ExamResult> CreateExamResultAsync(ExamResult examResult)
        {
            HttpResponseMessage response = null;
            try
            {
                var json = JsonSerializer.Serialize(examResult);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                response = await _httpClient.PostAsync("api/ExamResult", content);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<ExamResult>();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        // Delete an exam results from the API
        public async Task DeleteExamResultAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/ExamResult/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
