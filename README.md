# 🤖 DogmaAgent

**DogmaAgent** é um agente cognitivo de governança e orquestração de regras desenvolvido em **C#** e integrado ao **Gemini 3.1 Pro**. O objetivo do sistema é gerenciar o ciclo de vida de "Dogmas" — diretrizes conceituais e pedagógicas — garantindo a integridade semântica da árvore de conhecimentos através de análise de impacto automatizada.

## 🧠 O que é um "Dogma"?

Neste ecossistema, um **Dogma** não é apenas um registro, mas uma unidade de diretriz que possui linhagem. Cada dogma pode ter dependentes (sucessores). Quando uma regra central é alterada, o Agente Dogma avalia o impacto em toda a cascata para evitar contradições teóricas ou práticas nos níveis de formação (**Sementinhas, Mensageiros e Atalaias**).


---
## 📄 Regras dos Dogmas

### 1. Um Dogma nunca pode ser contrariado, mas pode ser revogado ou substituído

### 2. Dogmas são hierárquicos. Se um dogma antecessor for revogado seus sucessores também o serão de forma arbitrária e irrevogável. Mas se um Dogma for substituído por outro, os sucessores do primeiro serão considerados como sucessores do segundo, a menos que sua natureza seja contrariada pela do novo antecessor, o que fará o sucessor ser revogado

### 3. Há obrigatoriamente um, e somente um, Dogma inicial, que terá a nomenclatura especial de 'Genesis'. Todos os demais Dogmas lhe serão sucessores e ele representa o início do contexto controlado pelo agente

---

## 🛠️ Mapeamento de Skills (Inventário Técnico)

O agente foi construído sobre uma arquitetura de métodos especializados que definem suas capacidades:

### 1. Inteligência e Análise Semântica
*   **Análise de Impacto em Lote (Batch Processing):** Através do método `PromptFactory.BuildBatchSubstitutionPrompt`, o agente empacota múltiplos dogmas para uma única avaliação semântica, otimizando custo e tempo de resposta da API.
*   **Interpretação de Contexto:** Utiliza o `GeminiService.AnalyzeAsync` para discernir se a natureza de um novo dogma pai invalida a premissa de seus sucessores, indo além da simples comparação de palavras-chave.

### 2. Governança Humano-no-Circuito (Human-in-the-Loop)
*   **Relatório de Impacto:** Antes de qualquer ação destrutiva, o agente compila um "Julgamento" detalhado com **Memória de Cálculo**, explicando logicamente por que cada regra deve ser mantida ou revogada.
*   **Chancela de Autoridade:** O sistema possui a capacidade de pausar a execução (`DogmaManager.SubstituteDogmaAsync`) e solicitar a aprovação explícita do usuário (PO/Guardião do Contexto) antes de consolidar mudanças no sistema de arquivos.

### 3. Gestão de Ciclo de Vida e Persistência
*   **Poda Recursiva (Cascade Revocation):** O método `RevokeDogmaCascade` executa uma varredura profunda na árvore de dependências, garantindo que nenhum dogma "órfão" ou contraditório permaneça ativo.
*   **Manipulação de Metadados YAML/Markdown:** O `DogmaRepository` realiza o parse bidirecional entre objetos C# e arquivos físicos `.md`, mantendo cabeçalhos estruturados para fácil leitura humana e processamento computacional.

### 4. Auditoria e Compliance
*   **Rastreabilidade de Evidências:** Cada revogação gera automaticamente um log na pasta `/Evidence`, registrando o *timestamp*, a justificativa da IA e o ID do dogma afetado.

---

## 🏗️ Estrutura do Projeto

*   **`DogmaNode`**: Modelo de dados (ID, Natureza, Conteúdo, ParentId).
*   **`DogmaRepository`**: Camada de persistência (FileSystem).
*   **`GeminiService`**: Ponte de comunicação com o motor Generative AI.
*   **`DogmaManager`**: Orquestrador central e lógica de Human-in-the-Loop.
*   **`PromptFactory`**: Engenharia de prompts para respostas estruturadas em JSON.

---

## 🚀 Como Executar

1.  **Configuração da API:**
    Obtenha uma chave no [Google AI Studio](https://aistudio.google.com/) e configure-a no `GeminiService.cs`.
    
2.  **Compilação:**
    ```bash
    dotnet build
    ```

3.  **Execução:**
    ```bash
    dotnet run
    ```

---
*Desenvolvido por Carlos - Engenharia de Software focada em **Em Tudo Amar e Servir**.*