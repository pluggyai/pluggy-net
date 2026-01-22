# Add New API Endpoint

Add a new endpoint to the Pluggy .NET SDK following established patterns.

## Arguments

- `$ARGUMENTS` - The endpoint name or description (e.g., "consents", "loans", "account statements")

## Instructions

1. **Research the endpoint**
   - Check https://api.pluggy.ai/oas3.json for the endpoint schema
   - Check https://docs.pluggy.ai for documentation
   - Identify: HTTP method, path, query params, request body, response schema

2. **Create/Update Models**

   For each new model needed, create file in `Pluggy.SDK/Model/`:

   ```csharp
   using System;
   using Newtonsoft.Json;

   namespace Pluggy.SDK.Model
   {
       public class ModelName
       {
           [JsonProperty("id")]
           public Guid Id { get; set; }

           // Add all properties from API schema
           // Use nullable types (?) for optional fields
       }
   }
   ```

3. **Create/Update Enums**

   For new enums, use the TolerantEnumConverter:

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

4. **Add URL constant to PluggyAPI.cs**

   ```csharp
   protected static readonly string URL_NEW_ENDPOINT = "/endpoint";
   ```

5. **Add API methods to PluggyAPI.cs**

   Follow existing patterns:

   ```csharp
   // List with pagination
   public async Task<PageResults<Model>> FetchModels(Guid itemId, Parameters pageParams = null)
   {
       var queryStrings = pageParams != null ? pageParams.ToQueryStrings() : new Dictionary<string, string>();
       queryStrings.Add("itemId", itemId.ToString());
       return await httpService.GetAsync<PageResults<Model>>(URL_PATH, null, queryStrings);
   }

   // Get by ID
   public async Task<Model> FetchModel(Guid id)
   {
       return await httpService.GetAsync<Model>(URL_PATH + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
   }

   // Create
   public async Task<Model> CreateModel(ModelRequest request)
   {
       return await httpService.PostAsync<Model>(URL_PATH, request.ToBody());
   }

   // Update
   public async Task<Model> UpdateModel(Guid id, ModelRequest request)
   {
       return await httpService.PatchAsync<Model>(URL_PATH + "/{id}", request.ToBody(), null, HTTP.Utils.GetSegment(id.ToString()));
   }

   // Delete
   public async Task DeleteModel(Guid id)
   {
       await httpService.DeleteAsync<dynamic>(URL_PATH + "/{id}", HTTP.Utils.GetSegment(id.ToString()), null);
   }
   ```

6. **Create Parameter class if needed**

   For endpoints with query parameters:

   ```csharp
   public class ModelParameters
   {
       public DateTime? DateFrom { get; set; }
       public DateTime? DateTo { get; set; }
       public int? Page { get; set; }
       public int? PageSize { get; set; }

       public Dictionary<string, string> ToQueryStrings()
       {
           return new Dictionary<string, string>
           {
               { "from", DateFrom?.ToString("yyyy-MM-dd") },
               { "to", DateTo?.ToString("yyyy-MM-dd") },
               { "page", Page?.ToString() },
               { "pageSize", PageSize?.ToString() }
           }.RemoveNulls();
       }
   }
   ```

7. **Update CLAUDE.md**

   Remove the implemented endpoint from the gap summary.

8. **Build and verify**

   ```bash
   dotnet build Pluggy.SDK/
   ```

## Checklist

- [ ] Models created with all properties from API schema
- [ ] Enums created with TolerantEnumConverter
- [ ] URL constant added to PluggyAPI.cs
- [ ] API methods follow existing patterns (async Task)
- [ ] Parameter class created if needed
- [ ] XML documentation comments added to public methods
- [ ] Build succeeds
- [ ] CLAUDE.md updated
