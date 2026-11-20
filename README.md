# .NET client library for the Pluggy API

**Welcome to Pluggy Dotnet SDK**. This repository contains Pluggy's Dotnet SDK and samples to execute all the endpoints available for aggregation.
Also there is a small Client created to review the flow to get the execution result of a specific robot.

### Installation

```powershell
Install-Package Pluggy.SDK
```

### Usage

Request a API KEY token that will be used for all API calls you wish to make.
Create an instance of the `PluggyAPI` class with the token and the API URL:

```csharp
var client = new PluggyAPI("YOUR_CLIENT_ID", "YOUR_CLIENT_SECRET");
```

All api calls are under the same client, and respect the same behaviour.

```csharp
var connectors = await client.FetchConnectors();
```

#### Security at transit

At Pluggy we enforce the use of TLSv1.2 or higher. If you are using .NET < 4.7.2, this won't be the default communication for http clients, and you will need to set it.

```csharp
System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
```

#### Handling API errors

Every API error is thrown as an `ApiException` (`NotFoundException` for 404, `ValidationException`
when the item parameters are invalid). `StatusCode` holds the HTTP status, `ErrorCode` the error code
the API returned in `codeDescription`, and `Message` its description; the full body is in `ApiError`.
Known codes are listed in `ApiErrorCode`, and the full list is in the
[error reference](https://docs.pluggy.ai/reference/error-codes).

```csharp
using Pluggy.SDK.Errors;

try
{
    var connectToken = await client.CreateConnectToken();
}
catch (ApiException e) when (e.HasErrorCode(ApiErrorCode.CLIENT_IP_NOT_ALLOWED))
{
    // 403: this server's IP is not in the application's "Allowed IPs" list (Dashboard).
    // Retrying will not help: add the IP to the list or call the API from an allowed server.
    logger.LogError("Pluggy rejected the request IP: {Message}", e.Message);
}
```

When the application has an IP allowlist, `CLIENT_IP_NOT_ALLOWED` is returned by the
authentication (the first call the client makes) and by every request made with the application's
API key, including `CreateConnectToken`. See the
[IP allowlist guide](https://docs.pluggy.ai/docs/security/ip-allowlist).

#### Verifying webhook signatures

When webhook signing is enabled for your application, every notification carries an
`X-Pluggy-Signature` header (`t=<timestamp>,v1=<signature>`). Verify it against the **raw**
request body (before any JSON parsing or model binding) with the signing secret shown in the
Dashboard. Pass more than one secret to keep accepting the old one while you rotate it.

```csharp
using System.Text.Json;
using Pluggy.SDK.Webhooks;

app.MapPost("/webhooks/pluggy", async (HttpRequest request) =>
{
    // Read the raw body: do not bind it to a model before verifying it.
    using var buffer = new MemoryStream();
    await request.Body.CopyToAsync(buffer);
    byte[] payload = buffer.ToArray();

    bool valid = WebhookSignature.Verify(
        payload,
        request.Headers[WebhookSignature.HeaderName].ToString(),
        // null and empty entries are skipped, so the old secret can simply be unset after the rotation
        Environment.GetEnvironmentVariable("PLUGGY_WEBHOOK_SECRET"),
        Environment.GetEnvironmentVariable("PLUGGY_WEBHOOK_SECRET_OLD"));
    if (!valid)
    {
        return Results.Unauthorized();
    }

    using var notification = JsonDocument.Parse(payload);
    // handle the event...
    return Results.Ok();
});
```

`WebhookSignature.Verify` returns `true` only when the header parses, its timestamp is within the
tolerance (5 minutes by default, in either direction) and the signature matches one of the
secrets; it never throws. Use `WebhookSignature.Validate` instead to get the reason of a failure
for your logs (`result.Reason`, or `result.ReasonCode`: `missing_header`, `malformed_header`,
`timestamp_out_of_tolerance`, `signature_mismatch`, `no_secret`, `invalid_payload`). Both accept
the body as `byte[]` or `string`, and an overload takes a list of secrets, a custom tolerance in
seconds and the current time.

See the [webhook security guide](https://docs.pluggy.ai/docs/developer-tools/webhook-security)
for details.

## Building

This project can be built on Windows, Linux or macOS. Ensure you have the [.NET Core SDK](https://www.microsoft.com/net/download) installed.

## Documentation

For most up-to-date and accurate documentation, please see our [API Reference](https://docs.pluggy.ai) page.
