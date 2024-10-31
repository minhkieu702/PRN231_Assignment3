using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;

namespace PE_PRN231_FA24_TrialTest_KieuQuangMinh_FE
{
    public static class Common
    {
        public static string BaseURL = "https://localhost:7226";

        private static readonly HttpClient httpClient = new() { BaseAddress = new Uri(BaseURL) };

        public static async Task<HttpResponseMessage> SendRequestAsync<T>(
        string url,
        HttpMethod method,
        T? body = default,
        string? jwt = null)
        {
            if (jwt != null && httpClient.DefaultRequestHeaders.Authorization?.Parameter != jwt)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            }

            using var request = new HttpRequestMessage(method, url);

            // Add JSON content if it's a POST or PUT request and body is provided
            if ((method == HttpMethod.Post || method == HttpMethod.Put) && body != null)
            {
                request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            }
            return await httpClient.SendAsync(request);
        }

        public async static Task<T?> ReadT<T>(HttpResponseMessage reponse)
        {
            var content = await reponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async static Task<string> ReadToken(HttpResponseMessage reponse)
        {
            var responseBody = await reponse.Content.ReadAsStringAsync();
            var token = JsonDocument.Parse(responseBody).RootElement.GetProperty("token").GetString();
            return token;
        }

        public async static Task<string> ReadError(HttpResponseMessage reponse)
        {
            var responseBody = await reponse.Content.ReadAsStringAsync();
            var errors = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
            var formattedErrors = new List<string>();
            if (errors != null && errors.ContainsKey("errors"))
            {
                var errorMessages = errors["errors"] as JsonElement?;
                if (errorMessages.HasValue)
                {
                    foreach (var error in errorMessages.Value.EnumerateObject())
                    {
                        foreach (var detail in error.Value.EnumerateArray())
                        {
                            formattedErrors.Add($"{error.Name}: {detail.GetString()}");
                        }
                    }
                }
            }
            return string.Join("<br/>", formattedErrors);
        }
    }
}
