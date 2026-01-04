# Context Hierarchy for Knowledge Organization

This document defines the contextual organization strategy for KnowU's knowledge representation system.

---

## Overview

KnowU uses a **three-level hierarchical context model** to organize and cluster knowledge:

1. **Domain** (most abstract) → "Engineering", "Product", "Operations"
2. **Context** (middle) → "KnowU Project", "Authentication Feature", "Q1 Planning"  
3. **Tags** (granular) → ["security", "OAuth", "API", "meeting-notes"]

This hierarchy enables powerful knowledge discovery, clustering, and thesaurus-based mapping.

---

## Architectural Assessment

### ✅ Why This Design Works

**1. Clustering & Discovery**
- Query across abstraction levels: "All security knowledge in Engineering domain"
- Find cross-cutting themes: "Authentication discussions across all projects"
- Build knowledge graphs: Related concepts across domains

**2. Thesaurus/Ontology Integration**
- **Hierarchical relations**: Domain → Context (broader/narrower)
- **Associative relations**: Tags ↔ Tags (synonyms, related-to)
- Example: Tags "auth" and "authentication" → map to canonical "Authentication"

**3. Scalability**
- Avoids flat tag chaos (1000s of unorganized tags)
- Natural navigation: drill down from broad → specific
- Multi-tenancy ready (different domains for different teams)

**4. Orthogonal to Ontology**
- **Ontology** (onto.json): Defines WHAT claims look like (predicates, entity types)
- **Context hierarchy**: Defines WHERE knowledge lives (organizational structure)
- They complement each other perfectly

---

## Practical Example

```
Note: "We decided to use OAuth2 for the mobile app"
├─ Domain: Engineering
├─ Context: Mobile App Project  
└─ Tags: [authentication, OAuth, security, decision]

Later queries:
- "All Engineering decisions" → domain-level query
- "Mobile App authentication" → context + tag query
- "OAuth across all projects" → tag-level cross-cutting search
```

---

## Design Considerations

### 1. Vocabulary Management

- **Domain/Context**: Should be **controlled** (predefined, curated)
- **Tags**: Can start **free-form**, later add thesaurus mapping
- **Prevents**: "auth" vs "authentication" vs "authn" chaos

### 2. Multiple Assignment

- Can a note span multiple contexts? (e.g., cross-team meeting)
- **Recommendation**: Primary context + additional tags for cross-cutting concerns

### 3. Evolution Strategy

- **Start**: Domain + Tags (two levels)
- **Add**: Context layer when collection has >20 notes
- **Later**: Thesaurus for tag normalization and synonym mapping

### 4. Storage Strategy

```csharp
// On Note (simple approach)
public class Note : IDocument
{
    public string Domain { get; init; }           // "Engineering"
    public string Context { get; init; }          // "KnowU Backend"
    public List<string> Tags { get; init; }       // ["security", "OAuth"]
}

// Inheritance: Claims reference Notes → inherit context automatically
```

### 5. Tag Thesaurus for Document Organization

**Purpose:** The thesaurus structures and normalizes document tags, NOT claim semantics.

**Key Distinction:**
- **Claim ontology** (onto.json): Defines semantic predicates and entity types for extracted knowledge
- **Tag thesaurus**: Organizes and normalizes document metadata for discovery and clustering

**Workflow:**
1. **Phase 1 (Current)**: Users apply free-form tags to documents (`List<string>`)
2. **Collection Phase**: Use the application to generate real content with natural tag usage
3. **Phase 2 (Future)**: Build thesaurus from observed patterns:
   - Canonical forms: "auth" → "authentication"
   - Synonyms: "OAuth2", "OAuth 2.0" → "OAuth"
   - Related tags: "authentication" ↔ "authorization"
   - Categories: Technical, Topical, Temporal, etc.

**Why Wait:**
- Let organic usage reveal actual tag patterns
- Avoid premature abstraction
- Build thesaurus from real data, not speculation

**Result:** Documents become discoverable through flexible, normalized tag queries while claims remain semantically precise through the ontology.

---

## Implementation Phases

### Phase 1: Foundation (Current Priority)
- Add Domain + Tags to IDocument
- User provides when creating note
- Claims inherit context from their source note
- Basic filtering by domain/tags

### Phase 2: Middle Layer
- Add Context intermediate layer
- Build tag thesaurus/normalization
- Implement context-aware querying
- Cross-context relationship tracking

### Phase 3: Advanced Discovery
- LLM-suggested tags (validates user input)
- Automatic clustering/topic modeling
- Cross-context knowledge discovery
- Thesaurus-based semantic search

---

## Integration Points

### With Note Layer
- Notes carry context metadata (domain, context, tags)
- Context is user-provided at creation time
- Immutable like note content itself

### With Claim Layer
- Claims inherit context from source note
- Enables context-filtered claim queries
- Cross-context claim analysis reveals broader patterns

### With Fact Layer
- Facts aggregate claims from multiple contexts
- Context tracking shows knowledge evolution across domains
- Helps identify domain-specific vs universal knowledge

### With Personas
- Different personas may have access to different domains
- Technical PM focuses on Engineering domain
- Business Analyst spans multiple domains
- Context filtering improves relevance

---

## Open Questions

1. **Initial domains**: What should the first 3-5 domains be?
   - Engineering, Product, Business, Operations, Research?

2. **Tag strategy**: Free-form initially, or predefine starter vocabulary?
   - Recommendation: Start free-form, build thesaurus from actual usage

3. **Multi-context notes**: Allow primary + secondary contexts?
   - Recommendation: Single primary context, use tags for cross-cutting

4. **Persona-domain mapping**: Should personas be restricted to specific domains?
   - Consider in Phase 2 when adding access control

---

## Related Documents

- [Synopsis.md](Synopsis.md) - Core knowledge representation overview
- [onto.json](../data/onto.json) - Ontology definition (predicates and entity types)
- [personas.yaml](../data/personas.yaml) - Agent persona definitions
