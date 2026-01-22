# Add Missing Model Fields

Add missing fields to existing SDK models based on API schema updates.

## Arguments

- `$ARGUMENTS` - The model name(s) to update (e.g., "Transaction", "Identity", "all")

## Instructions

1. **Get current API schema**
   - Fetch from https://api.pluggy.ai/oas3.json
   - Extract the schema for the specified model(s)

2. **Read current SDK model**
   - Location: `Pluggy.SDK/Model/{ModelName}.cs`
   - List all existing properties

3. **Compare and identify missing fields**

   Common fields to check:

   | Model | Potentially Missing Fields |
   |-------|---------------------------|
   | Transaction | operationType |
   | TransactionCreditCardMetadata | billId, cardNumber |
   | Identity | createdAt, updatedAt, establishmentCode, establishmentName |
   | InvestmentTransaction | agreedRate |
   | Investment | issuerCNPJ |
   | Address | additionalInfo |
   | Account | new bankData/creditData fields |
   | Connector | new feature flags |

4. **Add missing fields**

   Follow existing patterns in the model:

   ```csharp
   [JsonProperty("fieldName")]
   public string FieldName { get; set; }

   // For optional fields
   [JsonProperty("optionalField")]
   public DateTime? OptionalField { get; set; }

   // For nested objects
   [JsonProperty("nestedObject")]
   public NestedType NestedObject { get; set; }

   // For collections
   [JsonProperty("items")]
   public List<ItemType> Items { get; set; }
   ```

5. **Type mapping reference**

   | API Type | C# Type |
   |----------|---------|
   | string | string |
   | string (uuid) | Guid |
   | string (date-time) | DateTime or DateTime? |
   | number | double or double? |
   | integer | int or int? |
   | boolean | bool or bool? |
   | array | List<T> or IList<T> |
   | enum | Custom enum with TolerantEnumConverter |

6. **Build and verify**

   ```bash
   dotnet build Pluggy.SDK/
   ```

7. **Update CLAUDE.md**

   Update the "Missing Model Fields" section in CLAUDE.md.

## Example

Adding `operationType` to Transaction:

```csharp
// In Transaction.cs, add:
[JsonProperty("operationType")]
public string OperationType { get; set; }
```
