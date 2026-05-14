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
}