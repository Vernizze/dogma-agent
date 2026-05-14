This README.md serves as the official specification for the Dogma Agent. It covers the conceptual foundation, the logical rules of the hierarchy, and the technical architecture for the implementation.

Dogma: The AI Guardian of Concepts
Dogma is an autonomous AI agent designed to act as a "Guardian of Integrity" for a specific knowledge context. Unlike a standard knowledge base, Dogma manages a hierarchical tree of principles where every concept is tied to an ancestor. It ensures that any evolution in requirements, business rules, or spiritual guidelines remains consistent with the core "Dogmas" of the system.

🎯 Vision
To provide a single source of truth (SSoT) where changes are not just recorded, but analyzed for impact, ensuring that the "spirit" of the system is preserved through logical rigor and auditable decision-making.

📜 The Three Golden Rules
The Dogma Agent operates under three fundamental laws of logic:

Immutability and Lifecycle: A Dogma can never be contradicted while active. It cannot be "edited"—it can only be Revoked (deleted) or Substituted (replaced by a new version).

The Genesis Principle: There is exactly one starting point called Genesis. It represents the root of the entire context. If the Genesis is revoked, the entire context and all its successors are destroyed.

Hierarchical Cascading:

Revocation: If an ancestor is revoked, all its successors are revoked arbitrarily and irrevocably.

Substitution: If an ancestor is substituted, successors are automatically evaluated. If their nature contradicts the new ancestor, they are revoked. If compatible, they are re-attached to the new version.

🧠 Key Features
1. Memory of Calculation (Rationale)
Every decision made by the agent—whether to keep, move, or revoke a dogma—must be accompanied by a "Memory of Calculation." This is a detailed logical justification explaining the semantic analysis performed by the AI, ensuring the system is never a "black box."

2. The "Curator" Alert System
When interacting with users (e.g., Product Owners proposing new features), Dogma acts as an attentive curator. It doesn't block progress but issues clear impact warnings:

🟢 Green (Evolutionary): No conflicts detected.

🟡 Yellow (Divergent): Requires substitution of a leaf dogma; isolated impact.

🔴 Red (Disruptive): High-impact change that triggers a cascade of revocations.

3. Human-in-the-Loop (HITL)
In cases of semantic ambiguity or high-risk changes (like Genesis substitution), Dogma pauses and requests human arbitration. It records these decisions as Evidence, building a "jurisprudence" of how principles should be interpreted.

🛠 Technical Architecture
Tech Stack
Engine: Gemini AI (Inference & Semantic Analysis).

Backend: C# (.NET) using HttpClient for API communication.

Storage: File-based system using Markdown (.md) files.

Metadata: YAML Frontmatter for tracking IDs, Parents, and Status.

File Structure
Plaintext
/DogmaStorage
  /Active        <-- Current living dogmas
  /Revoked       <-- Historical record of deleted dogmas
  /Replaced      <-- Old versions of substituted dogmas
  /Evidence      <-- Decision logs and human intervention records
Data Schema (YAML)
Each .md file contains a header with its structural metadata:

YAML
---
id: DOGMA-001
name: "Data Privacy"
parentId: GENESIS
status: Active
nature: "Security and Compliance"
createdAt: 2026-05-14
---
🚀 Use Case: Software Development Governance
In a development environment, Dogma sits between the Product Owner (PO) and the Development Team.

Requirement Proposal: The PO proposes a new User Story.

Dogma Analysis: Dogma checks the story against the active tree.

Impact Report: Dogma warns if the story requires changing a fundamental principle (e.g., "This feature requires external data sharing, which contradicts Dogma ID-05: 'Internal Data Sovereignty'").

Alignment: The PO either adjusts the story or confirms the "Dogmatic Evolution," updating the context for all future stories.

📅 Roadmap
[ ] Phase 1: C# Console POC with basic file CRUD and Gemini integration.

[ ] Phase 2: Implementation of recursive cascading revocation logic.

[ ] Phase 3: Visual Tree mapping (graphing the dependencies).

[ ] Phase 4: VS Code Extension for real-time "Dogma Compliance" checks while writing requirements.

Created by: Carlos
Date: May 2026
Status: Conceptual Design Complete / Implementation Starting