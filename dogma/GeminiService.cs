using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace dogma;

public class GeminiService(string apiKey)
{
    private readonly string _apiKey = apiKey;
    private readonly HttpClient _httpClient = new();
    private const string ApiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-flash-latest:generateContent";
    //private const string ApiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent";

    public async Task DiscoverModelsAsync()
    {
        Console.WriteLine("Buscando modelos disponíveis para a sua chave...");
        var response = await _httpClient.GetAsync($"https://generativelanguage.googleapis.com/v1beta/models?key={_apiKey}");
        var result = await response.Content.ReadAsStringAsync();

        // Imprime o JSON bruto no console
        Console.WriteLine(result);
    }

    public async Task<string> AnalyzeAsync(string context, string prompt)
    {
        var payload = new
        {
            contents = new[] { new { parts = new[] { new { text = prompt } } } }
        };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{ApiUrl}?key={_apiKey}", content);
        var result = await response.Content.ReadAsStringAsync();

        // Faz o parse do JSON de forma segura
        using var doc = JsonDocument.Parse(result);

        // 1. Verifica se a API devolveu um objeto de "error"
        if (doc.RootElement.TryGetProperty("error", out JsonElement errorElement))
        {
            var code = errorElement.GetProperty("code").GetInt32();
            var message = errorElement.GetProperty("message").GetString();
            throw new Exception($"Erro da API do Gemini (Código {code}): {message}");
        }

        // 2. Tenta extrair a resposta se deu sucesso
        if (doc.RootElement.TryGetProperty("candidates", out JsonElement candidatesElement))
        {
            return candidatesElement[0]
                                  .GetProperty("content")
                                  .GetProperty("parts")[0]
                                  .GetProperty("text").GetString() ?? "Erro ao ler o texto gerado.";
        }

        // 3. Se não for nem erro nem sucesso conhecido, imprime o que veio
        throw new Exception($"Estrutura de JSON inesperada:\n{result}");
    }

    public async Task<float[]> GetEmbeddingAsync(string text)
    {
        // Fallback de segurança: usando o text-embedding-004, que é garantido em todas as contas e regiões
        // AGORA SIM, o fallback correto!
        var modelName = "embedding-001";
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/{modelName}:embedContent?key={_apiKey}";

        var requestBody = new
        {
            model = $"models/{modelName}",
            content = new { parts = new[] { new { text = text } } }
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);

        // Mostra o erro real no console se falhar, em vez de quebrar silenciosamente
        if (!response.IsSuccessStatusCode)
        {
            var errorJson = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro na API de Embeddings: {errorJson}");
        }

        var jsonString = await response.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(jsonString);

        // Navega no JSON da Google para extrair o array de floats
        var values = jsonDoc.RootElement
            .GetProperty("embedding")
            .GetProperty("values")
            .EnumerateArray()
            .Select(x => x.GetSingle())
            .ToArray();

        return values;
    }
}