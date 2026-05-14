using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dogma;

public static class PromptFactory
{
    // Método para o cenário da Fase 1 (Proposta de novo Card/Dogma)
    public static string BuildImpactAnalysisPrompt(string currentTree, string proposal)
    {
        return $@"
        Você é o 'Dogma', o Agente Guardião de Conceitos.
        Analise o impacto da proposta abaixo contra a Árvore de Dogmas atual.
        
        ÁRVORE ATUAL:
        {currentTree}

        PROPOSTA DO USUÁRIO:
        {proposal}

        FORMATE SUA RESPOSTA COMO UM ALERTA DE CURADORIA:
        [Nível: Verde/Amarelo/Vermelho]
        Memória de Cálculo: (Explique sua lógica passo a passo)";
    }

    // Futuro Método para a Fase 3 (Substituição)
    public static string BuildSubstitutionAnalysisPrompt(string newParent, string successorToTest)
    {
        // Aqui entrará a lógica de comparação direta entre dois dogmas...
        return "";
    }

    public static string BuildBatchSubstitutionPrompt(string newParentDogma, string successorsList)
    {
        return $@"
                Você é o 'Dogma', o Agente Guardião de Conceitos.
                O Dogma Pai listado abaixo está substituindo uma versão antiga. Você deve analisar a compatibilidade dos dogmas sucessores com esta nova versão.

                NOVO DOGMA PAI (A Nova Verdade):
                {newParentDogma}

                DOGMAS SUCESSORES (A serem avaliados):
                {successorsList}

                REGRA DE AVALIAÇÃO:
                Para cada sucessor, determine se sua natureza ou regra específica é CONTRARIADA pelo Novo Dogma Pai. 
                Se a premissa do sucessor se tornar inválida ou inviável, ele deve ser revogado. Caso contrário, deve ser mantido.

                Você DEVE retornar APENAS um array JSON válido, sem formatação markdown (sem ```json), com a seguinte estrutura:
                [
                    {{
                        ""Id"": ""DOGMA-XXX"",
                        ""Acao"": ""MANTER"" ou ""REVOGAR"",
                        ""MemoriaCalculo"": ""Explicação lógica detalhada da decisão.""
                    }}
                ]";
    }

    public static string BuildEntityExtractionPrompt(string dogmaContent)
    {
        return $@"
                Você é um Arquiteto de Software sênior especialista em Domain-Driven Design (DDD).
                Sua tarefa é analisar o seguinte texto de Regra de Negócio e extrair o modelo de domínio (Business ERD) subjacente.

                TEXTO DA REGRA:
                ""{dogmaContent}""

                INSTRUÇÕES DE EXTRAÇÃO:
                1. Identifique os Atores (quem executa a ação), Entidades (objetos com ciclo de vida) e Artefatos (o que é gerado ou consumido).
                2. Identifique os verbos de transação que definem a cardinalidade e o Relacionamento de Negócio entre esses elementos.
                3. Gere um diagrama Entity-Relationship (ER) usando estritamente a sintaxe do Mermaid.js.
                4. Mantenha os nomes das entidades em PascalCase e no idioma original do texto.
                5. Retorne APENAS o código Mermaid puro, sem nenhuma marcação markdown (como ```mermaid), sem a palavra 'erDiagram' no início e sem nenhuma explicação adicional.

                EXEMPLO DE ESTRUTURA ESPERADA:
                    ATOR ||--o{{ ARTEFATO : processa
                    ENTIDADE_PRINCIPAL ||--|| ENTIDADE_SECUNDARIA : contem
                ";
    }

    public static string BuildGlobalDomainMapPrompt(List<DogmaNode> dogmas)
    {
        var prompt = @"
                    Você é um Arquiteto de Software sênior especialista em Domain-Driven Design (DDD).
                    Sua tarefa é ler um CONJUNTO de Regras de Negócio de uma empresa, realizar a resolução de entidades (consolidando pronomes e sinônimos como 'Empresa', 'Ela', 'Hollywood' e 'Locadora' em uma única entidade raiz) e gerar um modelo de domínio (Business ERD) ÚNICO, coeso e unificado.

                    REGRAS DE NEGÓCIO ATIVAS (CONTEXTO GLOBAL):
                    ";
                            foreach (var dogma in dogmas)
                            {
                                prompt += $"- [{dogma.Id}] {dogma.Content}\n";
                            }

                            prompt += @"
                    INSTRUÇÕES DE EXTRAÇÃO:
                    1. Analise o contexto global para unificar as entidades. Não crie entidades redundantes.
                    2. Identifique Atores, Entidades Principais e Artefatos.
                    3. Gere um diagrama Entity-Relationship (ER) consolidado usando estritamente a sintaxe do Mermaid.js.
                    4. Mantenha os nomes em PascalCase (ex: Cliente, Filme, Locacao).
                    5. Retorne APENAS o código Mermaid puro, sem marcações markdown, sem a palavra 'erDiagram' e sem texto adicional.
                    ";
        return prompt;
    }

    public static string BuildParentRoutingPrompt(string newRuleContent, List<DogmaNode> top3Candidates)
    {
        var prompt = $@"
                    Sua tarefa é definir qual é a regra PAI (fundamento lógico) de uma NOVA REGRA, baseando-se apenas nos 3 candidatos abaixo.

                    NOVA REGRA A SER INSERIDA:
                    ""{newRuleContent}""

                    CANDIDATOS (TOP 3 SIMILARIDADE SEMÂNTICA):
                    ";
                            foreach (var cand in top3Candidates)
                            {
                                prompt += $"- ID: {cand.Id} | Conteúdo: {cand.Content}\n";
                            }

                            prompt += @"
                    INSTRUÇÕES:
                    1. Escolha QUAL dos candidatos acima serve como guarda-chuva lógico ou premissa para a nova regra.
                    2. Retorne APENAS um JSON no formato: { ""ParentId"": ""ID_ESCOLHIDO"" }
                    3. Se NENHUM fizer sentido absoluto, retorne { ""ParentId"": ""ROOT"" }
                    ";
        return prompt;
    }

    public static string BuildGlobalRoutingPrompt(string newRuleContent, List<DogmaNode> activeDogmas)
    {
        var prompt = $@"
                        Sua tarefa é atuar como um Roteador Ontológico. Você deve definir qual é a regra PAI (fundamento lógico) de uma NOVA REGRA, analisando a árvore atual de regras do negócio.

                        NOVA REGRA A SER INSERIDA:
                        ""{newRuleContent}""

                        ÁRVORE DE REGRAS ATIVAS (CANDIDATOS):
                        ";
                                foreach (var dogma in activeDogmas)
                                {
                                    prompt += $"- ID: {dogma.Id} | Conteúdo: {dogma.Content}\n";
                                }

                                prompt += @"
                        INSTRUÇÕES:
                        1. Encontre qual regra ativa atua como categoria superior, premissa ou guarda-chuva lógico para a Nova Regra.
                        2. Retorne APENAS um JSON no formato estrito: { ""ParentId"": ""ID_ESCOLHIDO"" }
                        3. Se NENHUMA regra ativa for uma premissa lógica para a nova regra, retorne { ""ParentId"": ""ROOT"" }
                        ";
        return prompt;
    }
}