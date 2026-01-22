# Pluggy .NET SDK - Claude Code Knowledge Base

## Project Overview

This is the official Pluggy .NET SDK for the Pluggy financial data API. The SDK provides strongly-typed access to Pluggy's REST API for financial data aggregation, payments, and Open Finance in Brazil.

- **Target Framework**: .NET Standard 2.0
- **Package**: Pluggy.SDK (NuGet)
- **License**: MIT

## Project Structure

```
/Pluggy.SDK/           # Main SDK library
  ├── PluggyAPI.cs     # Main API client with all public methods
  ├── HTTP/            # HTTP communication layer (APIService.cs)
  ├── Model/           # Data models and enums
  ├── Errors/          # Exception classes
  ├── Helpers/         # Utility classes
  └── Utils/           # JSON converters (TolerantEnumConverter)
/Pluggy.Tests/         # Test project
/Pluggy.Client/        # Sample client application
```

## API Reference

- **OpenAPI Spec**: https://api.pluggy.ai/oas3.json
- **Documentation**: https://docs.pluggy.ai
- **Base URL**: https://api.pluggy.ai/

## SDK Sync Process

When syncing the SDK with the API, follow these steps:

### 1. Fetch Current API Specification
```
GET https://api.pluggy.ai/oas3.json
```

### 2. Compare Endpoints

Current SDK endpoints in `PluggyAPI.cs`:

| Resource | SDK Method | API Endpoint |
|----------|------------|--------------|
| Connectors | `FetchConnectors()`, `FetchConnector()` | GET /connectors |
| Connector Validation | `ValidateCredentials()` | POST /connectors/{id}/validate |
| Items | `CreateItem()`, `FetchItem()`, `UpdateItem()`, `DeleteItem()` | /items |
| Item MFA | `UpdateItemMFA()` | POST /items/{id}/mfa |
| Accounts | `FetchAccounts()`, `FetchAccount()` | /accounts |
| Transactions | `FetchTransactions()`, `FetchTransaction()` | /transactions |
| Investments | `FetchInvestments()`, `FetchInvestment()` | /investments |
| Investment Transactions | `FetchInvestmentTransactions()` | GET /investments/{id}/transactions |
| Identity | `FetchIdentity()`, `FetchIdentityByItemId()` | /identity |
| Categories | `FetchCategories()`, `FetchCategory()` | /categories |
| Webhooks | Full CRUD | /webhooks |
| Connect Token | `CreateConnectToken()` | POST /connect_token |

### 3. Known API Endpoints (for gap analysis)

#### Core Data Endpoints
- GET /connectors, GET /connectors/{id}, POST /connectors/{id}/validate
- POST /items, GET /items/{id}, PATCH /items/{id}, DELETE /items/{id}
- POST /items/{id}/mfa, PATCH /items/{id}/disable-auto-sync
- GET /accounts, GET /accounts/{id}, GET /accounts/{id}/statements
- GET /transactions, GET /transactions/{id}, PATCH /transactions/{id}
- GET /investments, GET /investments/{id}, GET /investments/{id}/transactions
- GET /identity, GET /identity/{id}
- GET /consents, GET /consents/{id}
- GET /loans, GET /loans/{id}
- GET /categories, GET /categories/{id}
- Full CRUD /webhooks
- POST /auth, POST /connect_token

#### Payment Initiation Endpoints
- /payments/recipients (CRUD + /pix-qr)
- /payments/requests (CRUD)
- /payments/intents (Create, Get)
- /payments/customers (CRUD)
- /payments/schedules

#### Boleto Management (Beta)
- POST /boletos, GET /boletos/{id}, POST /boletos/{id}/cancel

### 4. Model Patterns

When creating new models, follow existing patterns:

```csharp
using System;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class ModelName
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("propertyName")]
        public string PropertyName { get; set; }

        // Use nullable types for optional fields
        [JsonProperty("optionalField")]
        public DateTime? OptionalField { get; set; }
    }
}
```

### 5. Enum Patterns

```csharp
using Newtonsoft.Json;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum EnumName
    {
        VALUE_ONE,
        VALUE_TWO
    }
}
```

For enums with custom serialization:
```csharp
using System.Runtime.Serialization;

[JsonConverter(typeof(TolerantEnumConverter))]
public enum ProductType
{
    [EnumMember(Value = "ACCOUNTS")]
    Accounts,

    [EnumMember(Value = "CREDIT_CARDS")]
    CreditCards
}
```

### 6. API Method Patterns

```csharp
// GET with path parameter
public async Task<Model> FetchModel(Guid id)
{
    return await httpService.GetAsync<Model>(URL_PATH + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
}

// GET with query parameters
public async Task<PageResults<Model>> FetchModels(Guid itemId, ModelParameters pageParams = null)
{
    var queryStrings = pageParams != null ? pageParams.ToQueryStrings() : new Dictionary<string, string>();
    queryStrings.Add("itemId", itemId.ToString());
    return await httpService.GetAsync<PageResults<Model>>(URL_PATH, null, queryStrings);
}

// POST with body
public async Task<Model> CreateModel(ModelRequest request)
{
    return await httpService.PostAsync<Model>(URL_PATH, request.ToBody());
}

// PATCH with body and path parameter
public async Task<Model> UpdateModel(Guid id, ModelRequest request)
{
    return await httpService.PatchAsync<Model>(URL_PATH + "/{id}", request.ToBody(), null, HTTP.Utils.GetSegment(id.ToString()));
}

// DELETE
public async Task DeleteModel(Guid id)
{
    await httpService.DeleteAsync<dynamic>(URL_PATH + "/{id}", HTTP.Utils.GetSegment(id.ToString()), null);
}
```

## Current SDK Gap Summary (Last Updated: 2026-01-22)

### Missing Endpoints
- Boleto Management: All /boletos/* endpoints (beta, intentionally not added)

Note: Account Statements and Item Disable Auto Sync are private/internal endpoints.

### Missing Model Fields
All model fields are now up to date.

### Missing Models
None - SDK is up to date with production API.

### Recently Added (Phase 1, 2 & 3)
**Phase 1 - Model Fields:**
- Transaction.operationType
- TransactionCreditCardMetadata.billId/cardNumber
- Identity.createdAt/updatedAt/establishmentCode/establishmentName
- InvestmentTransaction.agreedRate
- Investment.issuerCNPJ
- Address.additionalInfo

**Phase 2 - Core Endpoints:**
- Consent model and endpoints (FetchConsents, FetchConsent)
- Loan model with all nested types and endpoints (FetchLoans, FetchLoan)
- UpdateTransaction method for category updates

**Phase 3 - Payment Initiation:**
- PaymentRecipient model + CRUD endpoints (Create, Fetch, FetchAll, Update, Delete)
- PaymentRequest model + endpoints (Create, Fetch, FetchAll, Delete)
- PaymentIntent model + endpoints (Create, Fetch, FetchAll)
- PaymentCustomer model + CRUD endpoints (Create, Fetch, FetchAll, Update, Delete)
- PaymentInstitution, PaymentCallbackUrls nested models
- PaymentAccountType, PaymentRequestStatus, PaymentIntentStatus, PaymentCustomerType enums
- Request classes: CreatePaymentRecipientRequest, CreatePaymentRequestRequest, CreatePaymentIntentRequest, CreatePaymentCustomerRequest

**Additional Fixes:**
- BoletoMetadata model added to TransactionPaymentData
- TransactionPaymentParticipant.routingNumberISPB field added

## Build and Test

```bash
# Build
dotnet build

# Test
dotnet test

# Pack
dotnet pack -c Release
```

## Version Management

Version is managed in `Pluggy.SDK/Pluggy.SDK.csproj` and follows semantic versioning.
Release workflow in `.github/workflows/` handles automated releases.
