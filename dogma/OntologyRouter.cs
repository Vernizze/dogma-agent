namespace dogma;

public class OntologyRouter
{
    private readonly GeminiService _geminiService;

    public OntologyRouter(GeminiService geminiService)
    {
        _geminiService = geminiService;
    }

    public async Task<string> FindParentIdAsync(DogmaNode newDogma, List<DogmaNode> activeDogmas)
    {
        Console.WriteLine($"\n[ROTEADOR COGNITIVO] Analisando contexto global para alocar a regra...");

        // Chama o prompt passando a regra nova e a árvore inteira
        var prompt = PromptFactory.BuildGlobalRoutingPrompt(newDogma.Content, activeDogmas);

        // Usamos o método AnalyzeAsync normal, que está liberado na sua chave
        var decisionJson = await _geminiService.AnalyzeAsync("Você é um arquiteto ontológico de dados.", prompt);

        return ExtractIdFromJson(decisionJson);
    }

    private string ExtractIdFromJson(string json)
    {
        try
        {
            json = json.Replace("```json", "").Replace("```", "").Trim();
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("ParentId").GetString();
        }
        catch
        {
            return "ROOT"; // Fallback seguro
        }
    }
}