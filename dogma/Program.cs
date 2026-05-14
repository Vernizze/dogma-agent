namespace dogma;

class Program
{
    static async Task Main(string[] args)
    {
        var repository = new DogmaRepository();

        // Coloque sua NOVA CHAVE aqui
        var gemini = new GeminiService("api-key");

        var dogmaManager = new DogmaManager(repository, gemini);
        var domainManager = new DomainMapManager(gemini);
        var router = new OntologyRouter(gemini);

        Console.WriteLine("🧹 Preparando terreno para Demonstração (Resetando pastas)...");
        if (Directory.Exists("DogmaStorage/Active")) Directory.Delete("DogmaStorage/Active", true);
        if (Directory.Exists("DogmaStorage/Domain")) Directory.Delete("DogmaStorage/Domain", true);
        Directory.CreateDirectory("DogmaStorage/Active");
        Directory.CreateDirectory("DogmaStorage/Domain");

        // Pegamos as regras totalmente órfãs (sem ParentId definido)
        var dogmasBrutos = GetHollywoodLocadoraRawData();

        Console.WriteLine("\n==================================================");
        Console.WriteLine("🌱 GÊNESE AUTÔNOMA: ROTEAMENTO VETORIAL E MER DINÂMICO");
        Console.WriteLine("==================================================");

        foreach (var dogma in dogmasBrutos)
        {
            Console.WriteLine($"\n>>> Processando nova regra: [{dogma.Id}] {dogma.Content}");

            var dogmasAtivos = repository.LoadAllActive();

            // ROTEAMENTO AUTÔNOMO
            if (dogmasAtivos.Count == 0)
            {
                // A primeira regra do sistema é, por definição da física, o ROOT
                dogma.ParentId = "ROOT";
                Console.WriteLine($"[ROTEADOR VETORIAL] Sistema vazio. Definindo {dogma.Id} como ROOT absoluto.");
            }
            else
            {
                // A IA descobre quem é o pai matemático/lógico
                dogma.ParentId = await router.FindParentIdAsync(dogma, dogmasAtivos);
                Console.WriteLine($"[ROTEADOR VETORIAL] Decisão Final: O pai lógico é o {dogma.ParentId}");
            }

            // Salva a regra já com a árvore definida
            repository.Save(dogma);

            // Atualiza o Mapa de Domínio Holístico
            var novoCenario = repository.LoadAllActive();
            await domainManager.ExtractGlobalDomainMapAsync(novoCenario);

            // Pausa estratégica de 3s para respeitar o Rate Limit (429) do Gemini
            await Task.Delay(3000);
        }

        Console.WriteLine("\n🎉 Demonstração da Gênese Concluída! Todo o ecossistema foi auto-roteado e mapeado.");

        // Se quiser rodar a Disrupção do DVD depois disso, o código antigo da Fase 2 pode entrar aqui!
    }

    // Observe que NENHUM dogma aqui possui ParentId. O Roteador Vetorial fará todo o trabalho.
    public static List<DogmaNode> GetHollywoodLocadoraRawData()
    {
        return new List<DogmaNode>
        {
            new DogmaNode { Id = "HL-001", Content = "A empresa Hollywood Locadora foi fundada em 13/05/1992." },
            new DogmaNode { Id = "HL-002", Content = "O CNPJ da Hollywood é 12.345.678-0001-01." },
            new DogmaNode { Id = "HL-003", Content = "A atividade principal dela é locação de filmes VHS." },
            new DogmaNode { Id = "HL-004", Content = "Ela também vende refrigerantes e snacks para os clientes como conveniência." },
            new DogmaNode { Id = "HL-005", Content = "Cada filme tem o custo de R$0,10 por dia se for do tipo Catálogo." },
            new DogmaNode { Id = "HL-006", Content = "Cada filme tem o custo de R$0,20 por dia se for do tipo Lançamento." },
            new DogmaNode { Id = "HL-009", Content = "Se um filme for devolvido sem rebobinar será cobrada uma multa de R$0,03." },
            new DogmaNode { Id = "HL-010", Content = "Se um filme for danificado pelo cliente ele deverá pagar um valor de 10 locações em seu valor sem descontos." },
            new DogmaNode { Id = "HL-007", Content = "Cada filme tem o custo de R$0,05 por dia se for do tipo Catálogo e for locado conjuntamente a, ao menos, outros 3 filmes também do tipo Catálogo." },
            new DogmaNode { Id = "HL-008", Content = "Cada filme tem o custo de R$0,15 por dia se for do tipo Lançamento e for locado conjuntamente a, ao menos, outros 5 filmes também do tipo Lançamento." },
            new DogmaNode { Id = "HL-011", Content = "O pacote de pipoca de microondas custa R$ 5,00 e o achocolatado custa R$ 3,50." }
        };
    }
}