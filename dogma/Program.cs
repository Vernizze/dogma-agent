namespace dogma;

class Program
{
    // Note que agora o Main é async Task para podermos usar o await na IA
    static async Task Main(string[] args)
    {
        Console.WriteLine("--- DOGMA AGENT: FASE 3 (SUBSTITUIÇÃO INTELIGENTE) ---");

        var repo = new DogmaRepository();
        var gemini = new GeminiService("API_KEY"); // Não esqueça a chave!

        var manager = new DogmaManager(repo, gemini);

        // 1. Setup: Criando o cenário base
        SetupPhase3TestData(repo);

        Console.WriteLine("\n[Estado Atual da Árvore (Pasta /Active)]");
        ListActiveFiles();

        Console.WriteLine("\n[Ação do PO] Propondo a Substituição do DOGMA-100...");
        Console.WriteLine("Pressione qualquer tecla para enviar a proposta ao motor da IA...");
        Console.ReadKey();

        // 2. A Nova Proposta de Dogma que vai substituir o antigo DOGMA-100
        var novaProposta = new DogmaNode
        {
            Id = "DOGMA-100", // Mantém o ID para indicar substituição
            Name = "Regra da Dupla Missão",
            ParentId = "GENESIS",
            Nature = "Metodologia de Envio",
            Content = "Toda missão de envio ao final do encontro deve ser obrigatoriamente dupla: AMAR (internidade e oração) e SERVIR (ação externa e concreta)."
        };

        // 3. Execução da Fase 3
        await manager.SubstituteDogmaAsync(novaProposta);

        // 4. Verificando o Resultado
        Console.WriteLine("\n[Estado Final da Árvore (Pasta /Active)]");
        ListActiveFiles();

        Console.WriteLine("\n[Arquivos Movidos para a Lixeira (Pasta /Revoked)]");
        ListRevokedFiles();

        Console.WriteLine("\nFim do teste. Verifique o output do console para ler a Memória de Cálculo da IA!");
    }

    static void SetupPhase3TestData(DogmaRepository repo)
    {
        ClearDirectory("DogmaStorage/Active");
        ClearDirectory("DogmaStorage/Revoked");
        ClearDirectory("DogmaStorage/Evidence");

        // Gênesis
        repo.Save(new DogmaNode
        {
            Id = "GENESIS",
            Name = "Espiritualidade Inaciana",
            ParentId = null,
            Nature = "Mística",
            Content = "O lema central é 'Em Tudo Amar e Servir'."
        });

        // O Dogma Antigo (Que será substituído)
        repo.Save(new DogmaNode
        {
            Id = "DOGMA-100",
            Name = "Envio Exclusivamente Externo",
            ParentId = "GENESIS",
            Nature = "Metodologia de Envio",
            Content = "A etapa de Envio é livre e focada APENAS em ações comunitárias externas."
        });

        // Filho 1 (Deve SOBREVIVER, pois é uma ação externa de SERVIR)
        repo.Save(new DogmaNode
        {
            Id = "DOGMA-101",
            Name = "Ação Solidária",
            ParentId = "DOGMA-100",
            Nature = "Prática",
            Content = "As crianças devem recolher alimentos para doação como parte do Envio."
        });

        // Filho 2 (Deve MORRER, pois contraria a nova exigência de oração interna/AMAR)
        repo.Save(new DogmaNode
        {
            Id = "DOGMA-102",
            Name = "Proibição Contemplativa",
            ParentId = "DOGMA-100",
            Nature = "Restrição de Tempo",
            Content = "É proibido usar o tempo do Envio para orações ou momentos contemplativos."
        });
    }

    // ... (Mantenha os métodos ListActiveFiles, ListRevokedFiles e ClearDirectory exatamente iguais ao código anterior) ...
    static void ListActiveFiles()
    {
        var files = Directory.GetFiles("DogmaStorage/Active", "*.md");
        foreach (var f in files) Console.WriteLine($" - {Path.GetFileName(f)}");
    }

    static void ListRevokedFiles()
    {
        var files = Directory.GetFiles("DogmaStorage/Revoked", "*.md");
        foreach (var f in files) Console.WriteLine($" - {Path.GetFileName(f)}");
    }

    static void ClearDirectory(string path)
    {
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        var dir = new DirectoryInfo(path);
        foreach (var file in dir.GetFiles()) file.Delete();
    }
}