namespace dogma;

class Program
{
    static async Task Main(string[] args)
    {
        var repository = new DogmaRepository();

        // Insira sua NOVA CHAVE aqui. 
        var gemini = new GeminiService("api-key");

        var dogmaManager = new DogmaManager(repository, gemini);
        var domainManager = new DomainMapManager(gemini);

        Console.WriteLine("🧹 Limpando o ecossistema (Resetando pastas)...");
        if (Directory.Exists("DogmaStorage/Active")) Directory.Delete("DogmaStorage/Active", true);
        if (Directory.Exists("DogmaStorage/Domain")) Directory.Delete("DogmaStorage/Domain", true);
        Directory.CreateDirectory("DogmaStorage/Active");
        Directory.CreateDirectory("DogmaStorage/Domain");

        var dogmasIniciais = GetHollywoodLocadoraData();

        Console.WriteLine("\n==================================================");
        Console.WriteLine("🌱 FASE 1: A GÊNESE DA LOCADORA (EVOLUÇÃO PASSO A PASSO)");
        Console.WriteLine("==================================================");

        foreach (var dogma in dogmasIniciais)
        {
            Console.WriteLine($"\n>>> Inserindo Regra Gênesis: [{dogma.Id}]...");

            // 1. Aqui é NASCIMENTO, não substituição. Salvamos direto.
            repository.Save(dogma);

            // 2. Extração e Atualização Imediata do MER de Negócios
            var dogmasAtivos = repository.LoadAllActive();
            await domainManager.ExtractGlobalDomainMapAsync(dogmasAtivos);

            // 3. Pausa estratégica de 3s para respeitar o Rate Limit (429) da API do Google
            await Task.Delay(3000);
        }

        Console.WriteLine("\n==================================================");
        Console.WriteLine("🎬 FASE 2: TESTE DE DISRUPÇÃO (A CHEGADA DO DVD)");
        Console.WriteLine("==================================================");

        var novaRegraDvd = new DogmaNode
        {
            Id = "HL-003",
            ParentId = "HL-001",
            Content = "A atividade principal dela é locação exclusiva de filmes em formato DVD. O formato VHS foi totalmente descontinuado."
        };

        // AQUI SIM usamos o Substitute, pois estamos trocando a HL-003 do VHS pela HL-003 do DVD
        await dogmaManager.SubstituteDogmaAsync(novaRegraDvd);

        Console.WriteLine("\n🗺️ Atualizando o MER de Negócios pós-Disrupção (Sincronização Ontológica)...");

        var dogmasSobreviventes = repository.LoadAllActive();
        await domainManager.ExtractGlobalDomainMapAsync(dogmasSobreviventes);

        Console.WriteLine("\n🎉 Sincronização Concluída! Todo o ciclo de vida foi mapeado e validado.");
    }

    // Transformamos o Setup em um "Fornecedor de Dados" limpo, sem responsabilidade de I/O
    public static List<DogmaNode> GetHollywoodLocadoraData()
    {
        return new List<DogmaNode>
        {
            new DogmaNode { Id = "HL-001", Content = "A empresa Hollywood Locadora foi fundada em 13/05/1992." },
            new DogmaNode { Id = "HL-002", ParentId = "HL-001", Content = "O CNPJ da Hollywood é 12.345.678-0001-01." },
            new DogmaNode { Id = "HL-003", ParentId = "HL-001", Content = "A atividade principal dela é locação de filmes VHS." },
            new DogmaNode { Id = "HL-004", ParentId = "HL-003", Content = "Ela também vende refrigerantes e snacks para os clientes como conveniência." },
            new DogmaNode { Id = "HL-005", ParentId = "HL-003", Content = "Cada filme tem o custo de R$0,10 por dia se for do tipo Catálogo." },
            new DogmaNode { Id = "HL-006", ParentId = "HL-003", Content = "Cada filme tem o custo de R$0,20 por dia se for do tipo Lançamento." },
            new DogmaNode { Id = "HL-009", ParentId = "HL-003", Content = "Se um filme for devolvido sem rebobinar será cobrada uma multa de R$0,03." },
            new DogmaNode { Id = "HL-010", ParentId = "HL-003", Content = "Se um filme for danificado pelo cliente ele deverá pagar um valor de 10 locações em seu valor sem descontos." },
            new DogmaNode { Id = "HL-007", ParentId = "HL-005", Content = "Cada filme tem o custo de R$0,05 por dia se for do tipo Catálogo e for locado conjuntamente a, ao menos, outros 3 filmes também do tipo Catálogo." },
            new DogmaNode { Id = "HL-008", ParentId = "HL-006", Content = "Cada filme tem o custo de R$0,15 por dia se for do tipo Lançamento e for locado conjuntamente a, ao menos, outros 5 filmes também do tipo Lançamento." },
            new DogmaNode { Id = "HL-011", ParentId = "HL-009", Content = "Se mais de um filme vier sem rebinar, cobrar o valor de R$0,05 por filme." }
        };
    }
}