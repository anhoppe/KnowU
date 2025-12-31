# Knowledge Representation Summary for LLM + Notes Project

This is a **concise, project-focused summary** of the most important subjects, explicitly centered on the **attributes of the proposed knowledge representation**.

---

## 1. Core Representation Strategy

**Principle:** Separate knowledge capture from knowledge commitment.

**Layers:**
1. **Note** – human input (ground truth)
2. **Claim** – LLM-extracted, uncertain statements
3. **Fact** – reviewed, curated, trusted knowledge

> This separation is the single most important architectural decision.

---

## 2. Notes (Immutable Source Layer)

**Purpose:** Preserve original human knowledge without interpretation loss.

**Required Attributes:**
- `id`
- `author`
- `timestamp`
- `text`
- `context` (project, team, meeting, etc.)

**Key Property:** Never modified; all downstream knowledge must reference notes.

---

## 3. Claims (Probabilistic Knowledge Layer)

**Purpose:** Represent what the system *thinks* might be true.

**Required Attributes:**
- `subject`
- `predicate` (from closed vocabulary)
- `object`
- `confidence`
- `modality` (asserted / negated / suspected)
- `sourceNote`
- `extractedBy` (model + version)
- `extractedAt`

**Critical Properties:**
- Claims are **not facts**
- Claims may contradict each other
- Claims must always be traceable

---

## 4. Facts (Curated Knowledge Layer)

**Purpose:** Represent stable, trusted knowledge.

**Required Attributes:**
- `subject`
- `predicate`
- `object`
- `derivedFrom` (claims)
- `approvedBy`
- `approvalTimestamp`
- `status` (active / deprecated)
- `validFrom` / `validTo`

**Key Property:** Scarce by design; promotion requires human or policy approval.

---

## 5. Predicates (Semantic Control Surface)

**Principle:** Closed world for predicates, open world for entities.

**Predicate Attributes:**
- `id`
- `label`
- `description`
- `domain`
- `range`

**Rules:**
- LLM selects from allowed predicates only
- `relatedTo` exists as a fallback
- New predicates require review

> This prevents semantic entropy.

---

## 6. Entities (Identity & Alignment)

**Two-Tier Entity Model:**
1. **CandidateEntity**
   - surface form
   - type guess
   - source note
2. **CanonicalEntity**
   - stable ID
   - primary label
   - aliases
   - type

**Key Attributes:**
- `aliases[]`
- `type`
- `description`
- `sameAs / possiblySameAs` (with confidence)

**Principle:** Identity is resolved, not assumed.

---

## 7. Provenance

Every Claim and Fact must include:
- `source`
- `who`
- `when`
- `how` (model / user / rule)

> Essential for trust and explainability.

---

## 8. Uncertainty & Modality

**Required Attributes:**
- `confidence` (numeric)
- `modality` (asserted, negated, hypothetical)

> If uncertainty is not explicit, the system lies.

---

## 9. Temporal Scope

**Required Attributes:**
- `createdAt`
- `validFrom`
- `validTo` (optional)

> Knowledge without time context becomes wrong silently.

---

## 10. OWL Usage (Strictly Limited)

**OWL Defines:**
- class hierarchy
- allowed relations
- domains and ranges
- equivalences (rare)

**OWL Does *Not* Contain:**
- extracted claims
- uncertain facts
- LLM output

> OWL is **schema**, not storage.

---

## 11. Validation (SHACL, Not Reasoning)

**SHACL Enforces:**
- predicate usage
- required provenance
- promotion rules
- structural correctness

> Validation > inference for this project.

---

## 12. Query & Consumption Model

**Supported Queries:**
- What is claimed?
- What is known?
- Why do we believe this?
- How confident are we?
- What changed over time?

LLMs consume via:
LLM ↔ SPARQL ↔ Knowledge Graph


---

## 13. Summary Table (Key Attributes)

| Aspect       | Required Attributes                                           |
|--------------|---------------------------------------------------------------|
| Notes        | author, timestamp, text                                       |
| Claims       | subject, predicate, object, confidence, source              |
| Facts        | triple, approval, provenance                                  |
| Predicates   | domain, range, description                                    |
| Entities     | canonical ID, aliases, type                                   |
| Provenance   | who, when, how                                                |
| Uncertainty  | confidence, modality                                          |
| Time         | validFrom, validTo                                            |

---

## Final Assessment

This representation:
- tolerates LLM noise
- supports human review
- avoids ontology collapse
- scales organizationally
- is explainable and auditable

> Next practical steps: define a minimal JSON-LD / RDF schema, a predicate vocabulary v1, or an end-to-end extraction example.
