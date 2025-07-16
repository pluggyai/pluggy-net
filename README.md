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

## Building

This project can be built on Windows, Linux or macOS. Ensure you have the [.NET Core SDK](https://www.microsoft.com/net/download) installed.

## Documentation

For most up-to-date and accurate documentation, please see our [API Reference](https://docs.pluggy.ai) page.
