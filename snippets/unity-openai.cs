using System.Text;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

// CODE VOORBEELD
// OpenAI API aanroepen om een tekst op te halen
// in het component zitten public variabelen voor de prompt, personality, voice, en api key


// package manager > install by name:
// com.unity.nuget.newtonsoft-json

public class OpenAIFetch
{
    private readonly string apiKey;
    private readonly HttpClient httpClient;
    private const string ApiUrl = "https://api.openai.com/v1/chat/completions";
    private const string Model = "gpt-3.5-turbo";
    private const float Temperature = 0.9f;

    public OpenAIFetch(string key, HttpClient client = null)
    {
        apiKey = string.IsNullOrWhiteSpace(key) ? throw new System.ArgumentException("API key required.") : key;
        httpClient = client ?? new HttpClient();
    }

    public async Task<string> SendRequestAsync(string prompt, string personality)
    {
        if (string.IsNullOrWhiteSpace(prompt))
        {
            Debug.LogError("Prompt is empty.");
            return null;
        }

        var payload = new
        {
            model = Model,
            temperature = Temperature,
            messages = new[]
            {
                new { role = "system", content = personality },
                new { role = "user", content = $"{prompt}. The result should be maximum 90 characters long. Do not use emoji." }
            }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, ApiUrl)
        {
            Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
        };
        request.Headers.Add("Authorization", $"Bearer {apiKey}");

        try
        {
            var response = await httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Debug.LogError($"API Error: {response.StatusCode} - {json}");
                return null;
            }

            return JObject.Parse(json)?["choices"]?[0]?["message"]?["content"]?.ToString();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Request failed: {ex.Message}");
            return null;
        }
    }
}
