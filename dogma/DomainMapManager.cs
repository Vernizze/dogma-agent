namespace dogma;

public class DomainMapManager
{
    private readonly GeminiService _geminiService;
    private const string DomainFolderPath = "DogmaStorage/Domain";
    private const string MermaidFilePath = "DogmaStorage/Domain/BusinessModel.mmd";

    public DomainMapManager(GeminiService geminiService)
    {
        _geminiService = geminiService;

        if (!Directory.Exists(DomainFolderPath))
        {
            Directory.CreateDirectory(DomainFolderPath);
        }
    }

    public async Task ExtractGlobalDomainMapAsync(List<DogmaNode> allActiveDogmas)
    {
        Console.WriteLine($"\n[DOMAIN MAP] Lendo contexto global de {allActiveDogmas.Count} dogmas para consolidar entidades...");

        var prompt = PromptFactory.BuildGlobalDomainMapPrompt(allActiveDogmas);

        try
        {
            var mermaidCode = await _geminiService.AnalyzeAsync("Você é um arquiteto de software consolidando um banco de dados relacional.", prompt);

            mermaidCode = mermaidCode.Replace("```mermaid", "").Replace("```", "").Replace("erDiagram", "").Trim();

            // Recria o arquivo do zero, pois estamos gerando a visão consolidada
            var content = "erDiagram\n    %% Visão Consolidada do Domínio\n    " + mermaidCode + "\n";
            File.WriteAllText(MermaidFilePath, content);

            Console.WriteLine("✅ Mapa de Domínio Unificado gerado com sucesso (BusinessModel.mmd).");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine(mermaidCode);
            Console.WriteLine("--------------------------------------------------");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro ao gerar Mapa Global: {ex.Message}");
        }
    }
}