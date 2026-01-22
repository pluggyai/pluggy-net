using Pluggy.SDK;
using Pluggy.SDK.Model;
using Xunit;
using Xunit.Abstractions;

namespace Pluggy.IntegrationTests;

/// <summary>
/// Integration tests that validate SDK endpoints against Pluggy's sandbox environment.
/// These tests create a real sandbox item, validate all endpoints, and clean up.
///
/// Required environment variables:
/// - PLUGGY_CLIENT_ID: Your Pluggy API client ID
/// - PLUGGY_CLIENT_SECRET: Your Pluggy API client secret
/// </summary>
public class SandboxIntegrationTests : IAsyncLifetime
{
    private readonly ITestOutputHelper _output;
    private readonly PluggyAPI _sdk;
    private Item? _item;

    // Pluggy Sandbox connector ID (Pluggy Bank)
    private const long SANDBOX_CONNECTOR_ID = 0;

    // Sandbox credentials
    private const string SANDBOX_USER = "user-ok";
    private const string SANDBOX_PASSWORD = "password-ok";

    public SandboxIntegrationTests(ITestOutputHelper output)
    {
        _output = output;

        var clientId = Environment.GetEnvironmentVariable("PLUGGY_CLIENT_ID")
            ?? throw new InvalidOperationException("PLUGGY_CLIENT_ID environment variable is required");
        var clientSecret = Environment.GetEnvironmentVariable("PLUGGY_CLIENT_SECRET")
            ?? throw new InvalidOperationException("PLUGGY_CLIENT_SECRET environment variable is required");

        _sdk = new PluggyAPI(clientId, clientSecret);
    }

    public async Task InitializeAsync()
    {
        _output.WriteLine("Creating sandbox item...");

        var parameters = new ItemParameters(SANDBOX_CONNECTOR_ID, new List<ItemParameter>
        {
            new ItemParameter("user", SANDBOX_USER),
            new ItemParameter("password", SANDBOX_PASSWORD)
        });

        _item = await _sdk.CreateItem(parameters);
        Assert.NotNull(_item);
        _output.WriteLine($"Item created with ID: {_item.Id}");

        // Wait for item to finish syncing
        var maxWaitTime = TimeSpan.FromMinutes(5);
        var startTime = DateTime.UtcNow;

        while (!_item.HasFinished())
        {
            if (DateTime.UtcNow - startTime > maxWaitTime)
            {
                throw new TimeoutException("Item sync timed out after 5 minutes");
            }

            await Task.Delay(PluggyAPI.STATUS_POLL_INTERVAL);
            _item = await _sdk.FetchItem(_item.Id);
            _output.WriteLine($"Item status: {_item.Status}");
        }

        Assert.Equal(ItemStatus.UPDATED, _item.Status);
        _output.WriteLine("Item sync completed successfully");
    }

    public async Task DisposeAsync()
    {
        if (_item != null)
        {
            _output.WriteLine($"Cleaning up - deleting item {_item.Id}");
            await _sdk.DeleteItem(_item.Id);
            _output.WriteLine("Item deleted successfully");
        }
    }

    [Fact]
    public async Task FetchConnectors_ReturnsConnectors()
    {
        var connectors = await _sdk.FetchConnectors(new ConnectorParameters { Sandbox = true });

        Assert.NotNull(connectors);
        Assert.NotEmpty(connectors.Results);
        _output.WriteLine($"Found {connectors.Total} sandbox connectors");
    }

    [Fact]
    public async Task FetchConnector_ReturnsSandboxConnector()
    {
        var connector = await _sdk.FetchConnector(SANDBOX_CONNECTOR_ID);

        Assert.NotNull(connector);
        Assert.True(connector.IsSandbox);
        _output.WriteLine($"Connector: {connector.Name}");
    }

    [Fact]
    public async Task FetchItem_ReturnsCreatedItem()
    {
        Assert.NotNull(_item);

        var fetchedItem = await _sdk.FetchItem(_item.Id);

        Assert.NotNull(fetchedItem);
        Assert.Equal(_item.Id, fetchedItem.Id);
        Assert.Equal(ItemStatus.UPDATED, fetchedItem.Status);
        _output.WriteLine($"Item fetched: {fetchedItem.Id}, Status: {fetchedItem.Status}");
    }

    [Fact]
    public async Task FetchAccounts_ReturnsAccounts()
    {
        Assert.NotNull(_item);

        var accounts = await _sdk.FetchAccounts(_item.Id);

        Assert.NotNull(accounts);
        Assert.NotEmpty(accounts.Results);

        foreach (var account in accounts.Results)
        {
            _output.WriteLine($"Account: {account.Id}, Number: {account.Number}, Balance: {account.Balance}");

            // Verify we can fetch individual account
            var fetchedAccount = await _sdk.FetchAccount(account.Id);
            Assert.NotNull(fetchedAccount);
            Assert.Equal(account.Id, fetchedAccount.Id);
        }
    }

    [Fact]
    public async Task FetchTransactions_ReturnsTransactions()
    {
        Assert.NotNull(_item);

        var accounts = await _sdk.FetchAccounts(_item.Id);
        Assert.NotEmpty(accounts.Results);

        var account = accounts.Results.First();
        var txParams = new TransactionParameters
        {
            DateFrom = DateTime.Now.AddYears(-1),
            DateTo = DateTime.Now
        };

        var transactions = await _sdk.FetchTransactions(account.Id, txParams);

        Assert.NotNull(transactions);
        _output.WriteLine($"Found {transactions.Total} transactions for account {account.Id}");

        if (transactions.Results.Any())
        {
            var tx = transactions.Results.First();
            _output.WriteLine($"Transaction: {tx.Id}, Date: {tx.Date}, Amount: {tx.Amount}, Description: {tx.Description}");

            // Verify we can fetch individual transaction
            var fetchedTx = await _sdk.FetchTransaction(tx.Id);
            Assert.NotNull(fetchedTx);
            Assert.Equal(tx.Id, fetchedTx.Id);
        }
    }

    [Fact]
    public async Task FetchInvestments_ReturnsInvestments()
    {
        Assert.NotNull(_item);

        var investments = await _sdk.FetchInvestments(_item.Id);

        Assert.NotNull(investments);
        _output.WriteLine($"Found {investments.Total} investments");

        foreach (var investment in investments.Results)
        {
            _output.WriteLine($"Investment: {investment.Id}, Name: {investment.Name}, Balance: {investment.Balance}");

            // Verify we can fetch individual investment
            var fetchedInvestment = await _sdk.FetchInvestment(investment.Id);
            Assert.NotNull(fetchedInvestment);
            Assert.Equal(investment.Id, fetchedInvestment.Id);

            // Fetch investment transactions
            var investmentTxs = await _sdk.FetchInvestmentTransactions(investment.Id);
            _output.WriteLine($"  Investment transactions: {investmentTxs.Total}");
        }
    }

    [Fact]
    public async Task FetchIdentity_ReturnsIdentity()
    {
        Assert.NotNull(_item);

        var identity = await _sdk.FetchIdentityByItemId(_item.Id);

        Assert.NotNull(identity);
        _output.WriteLine($"Identity: {identity.FullName}, Document: {identity.Document}");

        // Verify we can fetch by identity ID
        var fetchedIdentity = await _sdk.FetchIdentity(identity.Id);
        Assert.NotNull(fetchedIdentity);
        Assert.Equal(identity.Id, fetchedIdentity.Id);
    }

    [Fact]
    public async Task FetchCategories_ReturnsCategories()
    {
        var categories = await _sdk.FetchCategories();

        Assert.NotNull(categories);
        Assert.NotEmpty(categories.Results);
        _output.WriteLine($"Found {categories.Total} categories");

        var category = categories.Results.First();
        _output.WriteLine($"Category: {category.Id}, Description: {category.Description}");

        // FetchCategory expects a Guid, so we try to parse the category ID
        if (Guid.TryParse(category.Id, out var categoryGuid))
        {
            var fetchedCategory = await _sdk.FetchCategory(categoryGuid);
            Assert.NotNull(fetchedCategory);
            Assert.Equal(category.Id, fetchedCategory.Id);
        }
    }

    [Fact]
    public async Task CreateConnectToken_ReturnsToken()
    {
        var response = await _sdk.CreateConnectToken(null, new ItemOptions { ClientUserId = "integration-test" });

        Assert.NotNull(response);
        Assert.NotEmpty(response.AccessToken);
        _output.WriteLine($"Connect token created successfully (length: {response.AccessToken.Length})");
    }

    [Fact]
    public async Task FetchConsents_ReturnsConsents()
    {
        Assert.NotNull(_item);

        var consents = await _sdk.FetchConsents(_item.Id);

        Assert.NotNull(consents);
        _output.WriteLine($"Found {consents.Total} consents for item");

        foreach (var consent in consents.Results)
        {
            _output.WriteLine($"Consent: {consent.Id}, Created: {consent.CreatedAt}, Expires: {consent.ExpiresAt}");
        }
    }

    [Fact]
    public async Task FetchLoans_ReturnsLoans()
    {
        Assert.NotNull(_item);

        var loans = await _sdk.FetchLoans(_item.Id);

        Assert.NotNull(loans);
        _output.WriteLine($"Found {loans.Total} loans for item");

        foreach (var loan in loans.Results)
        {
            _output.WriteLine($"Loan: {loan.Id}, Product: {loan.ProductName}, Amount: {loan.ContractAmount}");
        }
    }

    [Fact]
    public async Task WebhookOperations_WorkCorrectly()
    {
        // Create webhook
        var webhook = await _sdk.CreateWebhook(
            "https://example.com/webhook-test",
            WebhookEvent.ITEM_UPDATED
        );

        Assert.NotNull(webhook);
        _output.WriteLine($"Webhook created: {webhook.Id}");

        try
        {
            // Fetch webhooks
            var webhooks = await _sdk.FetchWebhooks();
            Assert.NotNull(webhooks);
            Assert.Contains(webhooks.Results, w => w.Id == webhook.Id);

            // Fetch single webhook
            var fetchedWebhook = await _sdk.FetchWebhook(webhook.Id);
            Assert.NotNull(fetchedWebhook);
            Assert.Equal(webhook.Id, fetchedWebhook.Id);

            // Update webhook
            var updatedWebhook = await _sdk.UpdateWebhook(
                webhook.Id,
                "https://example.com/webhook-test-updated",
                WebhookEvent.ITEM_ALL
            );
            Assert.NotNull(updatedWebhook);
        }
        finally
        {
            // Clean up - delete webhook
            await _sdk.DeleteWebhook(webhook.Id);
            _output.WriteLine($"Webhook deleted: {webhook.Id}");
        }
    }
}
