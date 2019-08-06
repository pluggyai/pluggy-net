# .NET client library for the Hermes API

__Welcome to Hermes Dotnet SDK__. This repository contains Hermes's Dotnet SDK and samples to execute all the endpoints available for aggregation.
Also there is a small Client created to review the flow to get the execution result of a specific robot.

### Installation

```powershell
Install-Package Hermes.SDK
```

### Usage

Request a API KEY token that will be used for all API calls you wish to make. 
Create an instance of the `HermesAPI` class with the token and the API URL:

```csharp
var client = new HermesAPI("your api token", "https://api.hermesapi.com/v1");
```

All api calls are under the same client, and respect the same behaviour.

```csharp
var robots = await client.FetchRobots();
```

## Building

This project can be built on Windows, Linux or macOS. Ensure you have the [.NET Core SDK](https://www.microsoft.com/net/download) installed.

## Documentation

For most up-to-date and accurate documentation, please see our [API Reference](https://docs.hermesapi.com) page.
