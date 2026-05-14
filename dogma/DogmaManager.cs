using System.Text.Json;

namespace dogma;

public class DogmaManager
{
    private readonly string _activePath = "DogmaStorage/Active";
    private readonly string _revokedPath = "DogmaStorage/Revoked";
    private readonly string _evidencePath = "DogmaStorage/Evidence";

    // 1. Adicionamos a referência ao repositório
    private readonly DogmaRepository _repository;

    private readonly GeminiService _geminiService;

    // 2. Injetamos o repositório via construtor
    public DogmaManager(DogmaRepository repository, GeminiService geminiService)
    {
        _repository = repository;
        _geminiService = geminiService;
        Directory.CreateDirectory(_activePath);
        Directory.CreateDirectory(_revokedPath);
        Directory.CreateDirectory(_evidencePath);
    }

    public void RevokeDogmaCascade(string targetId, string reason)
    {
        if (targetId.Equals("GENESIS", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Falha Crítica: O Gênesis não pode ser revogado por este método.");
        }

        // 3. Agora chamamos o método através da instância do repositório
        var activeDogmas = _repository.LoadAllActive();
        var targetDogma = activeDogmas.FirstOrDefault(d => d.Id == targetId);

        if (targetDogma == null)
        {
            Console.WriteLine($"Dogma {targetId} não encontrado.");
            return;
        }

        var revokedList = new List<DogmaNode>();

        ExecuteRevocation(targetDogma, activeDogmas, revokedList, reason);
        GenerateRevocationEvidence(targetDogma, revokedList, reason);
    }

    private void ExecuteRevocation(DogmaNode current, List<DogmaNode> allActive, List<DogmaNode> revokedLog, string reason)
    {
        var successors = allActive.Where(d => d.ParentId == current.Id).ToList();

        foreach (var successor in successors)
        {
            ExecuteRevocation(successor, allActive, revokedLog, "Revogado por cascata do antecessor: " + current.Id);
        }

        MoveToRevoked(current, reason);
        revokedLog.Add(current);
    }

    private void MoveToRevoked(DogmaNode dogma, string reason)
    {
        var activeFile = Path.Combine(_activePath, $"{dogma.Id}.md");
        var revokedFile = Path.Combine(_revokedPath, $"{dogma.Id}.md");

        if (File.Exists(activeFile))
        {
            var updatedContent = File.ReadAllText(activeFile) + $"\n\n> **REVOGADO:** {reason}";
            File.WriteAllText(revokedFile, updatedContent);
            File.Delete(activeFile);
        }
    }

    private void GenerateRevocationEvidence(DogmaNode rootRevoked, List<DogmaNode> totalRevoked, string reason)
    {
        var evidenceFile = Path.Combine(_evidencePath, $"EV-REV-{DateTime.UtcNow:yyyyMMddHHmmss}.md");
        var sb = new System.Text.StringBuilder();

        sb.AppendLine($"# Registro de Evidência de Revogação");
        sb.AppendLine($"**Data:** {DateTime.Now}");
        sb.AppendLine($"**Alvo Principal:** {rootRevoked.Id} - {rootRevoked.Name}");
        sb.AppendLine($"**Motivo Base:** {reason}");
        sb.AppendLine();
        sb.AppendLine($"### Impacto em Cascata ({totalRevoked.Count} dogmas afetados):");

        foreach (var d in totalRevoked)
        {
            sb.AppendLine($"- {d.Id}: {d.Name}");
        }

        File.WriteAllText(evidenceFile, sb.ToString());
    }

    public async Task SubstituteDogmaAsync(DogmaNode newParentProposal)
    {
        var activeDogmas = _repository.LoadAllActive();
        var oldParent = activeDogmas.FirstOrDefault(d => d.Id == newParentProposal.Id);

        if (oldParent == null)
        {
            Console.WriteLine($"Erro: Dogma {newParentProposal.Id} não encontrado para substituição.");
            return;
        }

        // 1. Encontra todos os sucessores diretos
        var directSuccessors = activeDogmas.Where(d => d.ParentId == oldParent.Id).ToList();

        if (!directSuccessors.Any())
        {
            // Se não tem filhos, apenas atualiza o arquivo
            _repository.Save(newParentProposal);
            Console.WriteLine($"Dogma {newParentProposal.Id} atualizado com sucesso. Nenhum sucessor para analisar.");
            return;
        }

        // 2. Prepara os dados para o Batch Process
        var parentContentText = $"[{newParentProposal.Nature}] {newParentProposal.Content}";

        var successorsStringBuilder = new System.Text.StringBuilder();
        foreach (var s in directSuccessors)
        {
            successorsStringBuilder.AppendLine($"ID: {s.Id} | Natureza: {s.Nature} | Conteúdo: {s.Content}");
        }

        // 3. Consulta o Motor de IA
        var prompt = PromptFactory.BuildBatchSubstitutionPrompt(parentContentText, successorsStringBuilder.ToString());

        Console.WriteLine("Consultando o Agente para Análise Semântica em Lote...");
        var jsonResponse = await _geminiService.AnalyzeAsync("", prompt); // O context está embutido no prompt

        // 4. Executa as decisões da IA
        try
        {
            jsonResponse = jsonResponse.Replace("```json", "").Replace("```", "").Trim();
            var decisions = JsonSerializer.Deserialize<List<ValidationResult>>(jsonResponse);

            if (decisions == null) throw new Exception("Falha ao interpretar as decisões da IA.");

            // 1. Fase de Apresentação (Aconselhamento)
            Console.WriteLine("\n==================================================");
            Console.WriteLine("🤖 RELATÓRIO DE IMPACTO DA IA (AGENTE DOGMA)");
            Console.WriteLine("==================================================");

            var dogmasToRevoke = new List<ValidationResult>();

            foreach (var decision in decisions)
            {
                if (decision.Acao == "REVOGAR")
                {
                    Console.WriteLine($"\n❌ CONFLITO DETECTADO NO SUCESSOR: {decision.Id}");
                    Console.WriteLine($"   Motivo: {decision.MemoriaCalculo}");
                    dogmasToRevoke.Add(decision);
                }
                else
                {
                    Console.WriteLine($"\n✅ SUCESSOR MANTIDO: {decision.Id}");
                    Console.WriteLine($"   Motivo: {decision.MemoriaCalculo}");
                }
            }

            Console.WriteLine("==================================================\n");

            // 2. O "Human-in-the-Loop" (A Decisão)
            if (dogmasToRevoke.Any())
            {
                Console.WriteLine($"⚠️ ATENÇÃO: Para aplicar a substituição, {dogmasToRevoke.Count} dogma(s) sucessor(es) precisam ser revogados em cascata.");
                Console.Write("Você, como Guardião do Contexto, aprova esta operação? (S/N): ");

                var resposta = Console.ReadLine();

                if (resposta?.Trim().ToUpper() != "S")
                {
                    Console.WriteLine("\n🚫 Operação ABORTADA pelo usuário. A Árvore de Dogmas não foi alterada.");
                    return; // Sai do método sem salvar nada
                }

                Console.WriteLine("\n✅ Operação APROVADA. Iniciando revogação em cascata...");

                // 3. Fase de Execução (Ação)
                foreach (var dogma in dogmasToRevoke)
                {
                    RevokeDogmaCascade(dogma.Id, dogma.MemoriaCalculo);
                }
            }

            // Salva o novo dogma pai
            _repository.Save(newParentProposal);
            Console.WriteLine($"\n💾 Substituição concluída. O novo dogma {newParentProposal.Id} foi salvo na raiz.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro crítico durante a avaliação de substituição: {ex.Message}");
        }
    }
}