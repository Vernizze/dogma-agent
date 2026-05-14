# 🤖 DogmaAgent

**DogmaAgent** é um agente cognitivo de governança e orquestração de regras de negócio desenvolvido em **C#** e integrado ao **Gemini API**. O objetivo do sistema é gerenciar o ciclo de vida de diretrizes sistêmicas, garantindo a integridade da arquitetura de software através de análise de impacto automatizada, geração dinâmica de ontologias e mapeamento de domínio em tempo real.

---

## 1. O Conceito Geral do "Dogma"

Neste ecossistema, um **Dogma** é uma unidade atômica de regra de negócio. Ele não é um simples registro de banco de dados, mas um "nó cognitivo" em uma árvore de dependências. 

Os dogmas moldam a realidade da aplicação. Quando uma regra central (Dogma Pai) sofre uma disrupção ou é alterada, o sistema não permite que regras dependentes (Dogmas Filhos) permaneçam ativas se entrarem em contradição lógica com a nova realidade. O agente atua como o guardião da coerência estrutural do negócio.

---

## 2. As 3 Regras dos Dogmas

O ciclo de vida das regras de negócio neste sistema obedece a três leis imutáveis:

1. **Um Dogma nunca pode ser contrariado, mas pode ser revogado ou substituído
2. **Dogmas são hierárquicos. Se um dogma antecessor for revogado seus sucessores também o serão de forma arbitrária e irrevogável. Mas se um Dogma for substituído por outro, os sucessores do primeiro serão considerados como sucessores do segundo, a menos que sua natureza seja contrariada pela do novo antecessor, o que fará o sucessor ser revogado
3. **Há obrigatoriamente um, e somente um, Dogma inicial, que terá a nomenclatura especial de 'Genesis'. Todos os demais Dogmas lhe serão sucessores e ele representa o início do contexto controlado pelo agente
---

## 3. Mecânica de Validação e Memória de Análise

A validação de conflitos é realizada pelo motor de IA generativa atuando como um Arquiteto de Software.

* **Análise de Impacto:** O sistema cruza o texto da nova regra com os dogmas sucessores da regra antiga, avaliando não apenas palavras-chave, mas a semântica da operação.
* **Memória de Cálculo (Julgamento):** Para cada regra afetada, a IA compila um relatório lógico detalhando *por que* a regra sobrevive ou *por que* se torna obsoleta.
* **Auditoria de Evidências:** Nenhuma exclusão é silenciosa. As decisões arquiteturais da IA são documentadas em arquivos de *log* físicos armazenados na pasta `/Evidence`, garantindo a rastreabilidade (Data, Regra Afetada, Justificativa, Novo Contexto).

---

## 4. Processo de Revogação e Substituição

A evolução do negócio exige disrupção. O agente orquestra essa mudança através de um fluxo *Human-in-the-Loop* (Humano-no-Circuito):

1. **Input da Disrupção:** Uma nova regra é submetida utilizando o ID da regra antiga que ela visa substituir.
2. **Avaliação Cognitiva:** O `DogmaManager` rastreia todos os "filhos" e "netos" da regra original e pede ao LLM para validá-los contra a nova realidade.
3. **Pausa para Chancela:** O agente interrompe a execução, exibe o Relatório de Impacto no console e solicita a aprovação explícita do *Product Owner* (PO) ou Engenheiro.
4. **Execução da Poda:** Após a aprovação (`S`), as regras incompatíveis são movidas do diretório `/Active` e desativadas em cascata.

---

## 5. Mecânica de Geração do Entity Map (Mapeamento de Domínio)

O DogmaAgent atua como um engenheiro de dados, traduzindo texto corrido em arquitetura de banco de dados através do `DomainMapManager`.

* **Processamento Holístico:** O agente lê toda a árvore de dogmas sobreviventes em um único lote (*Batch Processing*), garantindo a resolução correta de entidades (ex: compreendendo que pronomes ocultos ou sinônimos referem-se à mesma tabela raiz).
* **Extração DDD:** Focado em *Domain-Driven Design*, o agente identifica Atores, Artefatos, Entidades e seus Relacionamentos de Cardinalidade.
* **Sincronização Visual:** O resultado é transpilado em tempo real para um diagrama de Entidade-Relacionamento utilizando a sintaxe **Mermaid.js**, que é atualizado dinamicamente na pasta `/Domain` sempre que o negócio sofre mutações.

---

## 6. Mecânica de Ontologia (Roteamento Autônomo)

O sistema possui capacidade de auto-organização, construindo a árvore hierárquica organicamente através do `OntologyRouter`.

* **Auto-Parenting:** Quando uma regra "órfã" (sem `ParentId` definido) é inserida no sistema, ela não precisa ser roteada manualmente pelo desenvolvedor.
* **Roteamento Cognitivo:** A IA lê a nova regra, lê toda a árvore de regras ativas e atua como um classificador semântico. Ela deduz matematicamente qual dogma ativo serve como "guarda-chuva" lógico ou premissa fundacional para a nova regra.
* **Estruturação Dinâmica:** O `ParentId` é atribuído autonomamente, e o dogma é enxertado no galho correto da árvore de arquitetura antes de ser salvo e submetido ao mapeamento de domínio.

---
*Desenvolvido para orquestração escalável de regras de negócio, governança de IA e modelagem autônoma de sistemas.*