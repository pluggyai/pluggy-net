# SDK API Synchronization

Analyze and synchronize the Pluggy .NET SDK with the current production API.

## Instructions

1. **Fetch the current OpenAPI specification**
   - URL: https://api.pluggy.ai/oas3.json
   - Extract all endpoints, methods, and schemas

2. **Read the current SDK implementation**
   - Main client: `Pluggy.SDK/PluggyAPI.cs`
   - Models: `Pluggy.SDK/Model/*.cs`
   - Check CLAUDE.md for known gaps and patterns

3. **Compare and identify gaps**

   ### Endpoints to check:
   - Auth: POST /auth, POST /connect_token
   - Connectors: GET /connectors, GET /connectors/{id}, POST /connectors/{id}/validate
   - Items: POST /items, GET /items/{id}, PATCH /items/{id}, DELETE /items/{id}, POST /items/{id}/mfa, PATCH /items/{id}/disable-auto-sync
   - Accounts: GET /accounts, GET /accounts/{id}, GET /accounts/{id}/statements
   - Transactions: GET /transactions, GET /transactions/{id}, PATCH /transactions/{id}
   - Investments: GET /investments, GET /investments/{id}, GET /investments/{id}/transactions
   - Identity: GET /identity, GET /identity/{id}
   - Consents: GET /consents, GET /consents/{id}
   - Loans: GET /loans, GET /loans/{id}
   - Categories: GET /categories, GET /categories/{id}
   - Webhooks: Full CRUD on /webhooks
   - Payments: /payments/recipients, /payments/requests, /payments/intents, /payments/customers
   - Boletos: /boletos endpoints

   ### Models to check for new fields:
   - Account, BankAccount, CreditAccount
   - Transaction, TransactionPaymentData, TransactionCreditCardMetadata, TransactionMerchant
   - Investment, InvestmentTransaction
   - Identity, Address, PhoneNumber, Email
   - Connector, ConnectorParameter
   - Item, ItemStatusDetail

4. **Generate gap report**

   Categorize findings into:
   - **Missing Endpoints**: API methods not in SDK
   - **Missing Model Fields**: Properties in API schema but not in SDK models
   - **Missing Models**: Entire entities not present in SDK
   - **Missing Enums**: Enumeration types not defined

5. **Propose implementation plan**

   Prioritize by:
   - Phase 1: Missing fields on existing models (low effort)
   - Phase 2: Core data endpoints (consents, loans, statements)
   - Phase 3: Payment initiation endpoints
   - Phase 4: Boleto management (beta features)

6. **Update CLAUDE.md**

   After analysis, update the "Current SDK Gap Summary" section in CLAUDE.md with new findings and date.

## Output Format

```markdown
## SDK Sync Report - [DATE]

### Missing Endpoints
| Priority | Endpoint | Method | Path |
|----------|----------|--------|------|
| ... | ... | ... | ... |

### Missing Model Fields
| Model | Missing Fields |
|-------|----------------|
| ... | ... |

### Missing Models
| Model | Description |
|-------|-------------|
| ... | ... |

### Implementation Plan
[Prioritized phases with effort estimates]
```
