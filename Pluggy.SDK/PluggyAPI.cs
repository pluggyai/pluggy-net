using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Pluggy.SDK.Errors;
using Pluggy.SDK.Helpers;
using Pluggy.SDK.HTTP;
using Pluggy.SDK.Model;

namespace Pluggy.SDK
{
    public class PluggyAPI
    {
        protected readonly APIService httpService;

        protected static readonly string URL_CONNECTORS = "/connectors";
        protected static readonly string URL_ITEMS = "/items";
        protected static readonly string URL_ACCOUNTS = "/accounts";
        protected static readonly string URL_TRANSACTIONS = "/transactions";
        protected static readonly string URL_INVESTMENTS = "/investments";
        protected static readonly string URL_CATEGORIES = "/categories";
        protected static readonly string URL_WEBHOOKS = "/webhooks";
        protected static readonly string URL_IDENTITY = "/identity";
        protected static readonly string URL_ITEMS_MFA = "/items/{id}/mfa";
        protected static readonly string URL_CONNECT_TOKEN = "/connect_token";
        protected static readonly string URL_INCOME_REPORT = "/income-reports";
        protected static readonly string URL_INVESTMENT_TRANSACTIONS = "/investments/{id}/transactions";

        public static readonly int STATUS_POLL_INTERVAL = 3000;


        public PluggyAPI(string _clientId, string _clientSecret, HttpClient _httpClient = null, string _baseUrl = "https://api.pluggy.ai/")
        {
            httpService = new APIService(_clientId, _clientSecret, _baseUrl, _httpClient);
        }

        public PluggyAPI(string _clientId, string _clientSecret, string _baseUrl)
            : this(_clientId, _clientSecret, null, _baseUrl)
        {
        }

        /// <summary>
        /// Fetch all available connectors
        /// </summary>
        /// <returns>An array of connectors</returns>
        public async Task<PageResults<Connector>> FetchConnectors(ConnectorParameters requestParams = null)
        {
            return await httpService.GetAsync<PageResults<Connector>>(URL_CONNECTORS, null, requestParams?.ToQueryStrings());
        }

        /// <summary>
        /// Fetch a single connector
        /// </summary>
        /// <param name="id">The connector ID</param>
        /// <returns>A connector object</returns>
        public async Task<Connector> FetchConnector(long id)
        {
            return await httpService.GetAsync<Connector>(URL_CONNECTORS + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Check that connector parameters are valid
        /// </summary>
        /// <param name="id">The connector ID</param>
        /// <param name="request">The connector parameters</param>
        /// <returns>an object with the info of which parameters are wrong</returns>
        public async Task<ValidationResult> ValidateCredentials(long id, List<ItemParameter> credentials)
        {
            try
            {
                return await httpService.PostAsync<ValidationResult>(
                    URL_CONNECTORS + "/{id}/validate",
                    credentials?.ToDictionary(x => x.Name, x => x.Value),
                    null,
                    HTTP.Utils.GetSegment(id.ToString())
                );
            }
            catch (ApiException e)
            {
                if (e.ApiError != null && e.ApiError.Errors != null)
                    throw new ValidationException(e.StatusCode, e.ApiError);

                throw e;
            }
        }

        /// <summary>
        /// Creates a new item for an specific connector
        /// </summary>
        /// <param name="request">The item parameters</param>
        /// <returns>an object with the info to retrieve the data when the execution is ready</returns>
        public async Task<Item> CreateItem(ItemParameters request)
        {
            try
            {
                return await httpService.PostAsync<Item>(URL_ITEMS, request.ToBody());
            }
            catch (ApiException e)
            {
                if (e.ApiError != null && e.ApiError.Errors != null)
                    throw new ValidationException(e.StatusCode, e.ApiError);

                throw e;
            }
        }

        /// <summary>
        /// Fetch a single item
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns></returns>
        public async Task<Item> FetchItem(Guid id)
        {
            return await httpService.GetAsync<Item>(URL_ITEMS + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Update an item connection with/without credentials forcing a sync
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns>An item object with the status of the connection</returns>
        public async Task<Item> UpdateItem(Guid id, ItemParameters request)
        {
            try
            {
                return await httpService.PatchAsync<Item>(URL_ITEMS + "/{id}", request.ToBody(), null, HTTP.Utils.GetSegment(id.ToString()));
            }
            catch (ApiException e)
            {
                if (e.ApiError != null && e.ApiError.Errors != null)
                    throw new ValidationException(e.StatusCode, e.ApiError);

                throw e;
            }
        }



        /// <summary>
        /// Sends multi-factor authentication parameter to item that is requesting it. 
        /// </summary>
        /// <param name="id">The item ID</param>
        /// <param name="parameters">Key-value pairs of requested parameters</param>
        /// <returns>An item object with the status of the connection</returns>
        public async Task<Item> UpdateItemMFA(Guid id, List<ItemParameter> parameters)
        {
            try
            {
                return await httpService.PostAsync<Item>(URL_ITEMS_MFA, parameters.ToDictionary(x => x.Name, x => x.Value), null, HTTP.Utils.GetSegment(id.ToString()));
            }
            catch (ApiException e)
            {
                if (e.ApiError != null && e.ApiError.Errors != null)
                    throw new ValidationException(e.StatusCode, e.ApiError);

                throw e;
            }
        }


        /// <summary>
        /// Deletes an Item by it's primary identifier
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns></returns>
        public async Task DeleteItem(Guid id)
        {
            await httpService.DeleteAsync<dynamic>(URL_ITEMS + "/{id}", HTTP.Utils.GetSegment(id.ToString()), null);
        }

        /// <summary>
        /// Fetch the list of accounts
        /// </summary>
        /// <param name="id">Item Id</param>
        /// <returns>Account results list</returns>
        public async Task<PageResults<Account>> FetchAccounts(Guid id, AccountType? type = null)
        {
            var queryStrings = new Dictionary<string, string>
            {
                { "itemId", id.ToString() },
                { "type", type.ToString() }
            };
            return await httpService.GetAsync<PageResults<Account>>(URL_ACCOUNTS, null, queryStrings);
        }

        /// <summary>
        /// Fetch the account details
        /// </summary>
        /// <param name="id">Account Id</param>
        /// <returns>Account details</returns>
        public async Task<Account> FetchAccount(Guid id)
        {
            return await httpService.GetAsync<Account>(URL_ACCOUNTS + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Fetch the list of transactions
        /// </summary>
        /// <param name="id">Account Id</param>
        /// <returns>Transacion results list</returns>
        public async Task<PageResults<Transaction>> FetchTransactions(Guid accountId, TransactionParameters pageParams = null)
        {
            var queryStrings = pageParams != null ? pageParams.ToQueryStrings() : new Dictionary<string, string>();
            queryStrings.Add("accountId", accountId.ToString());
            return await httpService.GetAsync<PageResults<Transaction>>(URL_TRANSACTIONS, null, queryStrings);
        }


        /// <summary>
        /// Fetch the list of transactions
        /// </summary>
        /// <param name="id">Account Id</param>
        /// <returns>Transacion results list</returns>
        public async Task<Transaction> FetchTransaction(Guid id)
        {
            return await httpService.GetAsync<Transaction>(URL_TRANSACTIONS + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Fetch the list of investments
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Invesments results list</returns>
        public async Task<PageResults<Investment>> FetchInvestments(Guid id, InvestmentType? type = null)
        {
            var queryStrings = new Dictionary<string, string>
            {
                { "itemId", id.ToString() },
                { "type", type.ToString() }
            };
            return await httpService.GetAsync<PageResults<Investment>>(URL_INVESTMENTS, null, queryStrings);
        }

        /// <summary>
        /// Fetch the Investment
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Investment details</returns>
        public async Task<Investment> FetchInvestment(Guid id)
        {
            return await httpService.GetAsync<Investment>(URL_INVESTMENTS + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
        }


        /// <summary>
        /// Fetch the Identity
        /// </summary>
        /// <param name="id">Identity resource id</param>
        /// <returns>Identity details</returns>
        public async Task<Identity> FetchIdentity(Guid id)
        {
            return await httpService.GetAsync<Identity>(URL_IDENTITY + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Fetch the Identity by the Item's ID
        /// </summary>
        /// <param name="id">Item resource id</param>
        /// <returns>Identity details</returns>
        public async Task<Identity> FetchIdentityByItemId(Guid id)
        {
            var queryStrings = new Dictionary<string, string>
            {
                { "itemId", id.ToString() }
            };
            return await httpService.GetAsync<Identity>(URL_IDENTITY, null, queryStrings);
        }

        /// <summary>
        /// Fetch the list of categories
        /// </summary
        /// <returns>Categories results list</returns>
        public async Task<PageResults<Category>> FetchCategories(string parentId = "")
        {
            var queryStrings = new Dictionary<string, string>() { { "parentId", parentId } };
            return await httpService.GetAsync<PageResults<Category>>(URL_CATEGORIES, null, queryStrings);
        }

        /// <summary>
        /// Fetch the category
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Investment details</returns>
        public async Task<Category> FetchCategory(Guid id)
        {
            return await httpService.GetAsync<Category>(URL_CATEGORIES + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Fetch the list of webhooks
        /// </summary
        /// <returns>Webhooks results list</returns>
        public async Task<PageResults<Webhook>> FetchWebhooks()
        {
            return await httpService.GetAsync<PageResults<Webhook>>(URL_WEBHOOKS);
        }

        /// <summary>
        /// Fetch the webhook
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Webhook details</returns>
        public async Task<Webhook> FetchWebhook(Guid id)
        {
            return await httpService.GetAsync<Webhook>(URL_WEBHOOKS + "/{id}", HTTP.Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Create a webhook
        /// </summary>
        /// <param name="url">Webhook url</param>
        /// <param name="event">Webhook event</param>
        /// <returns></returns>
        public async Task<Webhook> CreateWebhook(string url, WebhookEvent _event)
        {
            try
            {
                var body = new Dictionary<string, string>
                {
                    { "url", url },
                    { "event", _event.Value }
                };
                return await httpService.PostAsync<Webhook>(URL_WEBHOOKS, body);
            }
            catch (ApiException e)
            {
                if (e.ApiError != null && e.ApiError.Errors != null)
                    throw new ValidationException(e.StatusCode, e.ApiError);

                throw e;
            }
        }

        /// <summary>
        /// Update a single webhook
        /// </summary>
        /// <param name="id">Webhook id</param>
        /// <param name="url">Webhook url</param>
        /// <param name="event">Webhook event</param>
        /// <returns></returns>
        public async Task<Webhook> UpdateWebhook(Guid id, string url, WebhookEvent _event)
        {
            try
            {
                var body = new Dictionary<string, string>
                {
                    { "url", url },
                    { "event", _event.Value }
                };
                return await httpService.PatchAsync<Webhook>(URL_WEBHOOKS + "/{id}", body, null, HTTP.Utils.GetSegment(id.ToString()));
            }
            catch (ApiException e)
            {
                if (e.ApiError != null && e.ApiError.Errors != null)
                    throw new ValidationException(e.StatusCode, e.ApiError);

                throw e;
            }
        }

        /// <summary>
        /// Deletes a Webhook by it's primary identifier
        /// </summary>
        /// <param name="id">Webhook id</param>
        /// <returns></returns>
        public async Task DeleteWebhook(Guid id)
        {
            await httpService.DeleteAsync<dynamic>(URL_WEBHOOKS + "/{id}", HTTP.Utils.GetSegment(id.ToString()), null);
        }

        /// <summary>
        /// Fetch Income Reports of an specific Item 
        /// </summary>
        /// <param name="itemId">Item id</param>
        /// <returns>Paginated Income reports</returns>
        public async Task<PageResults<IncomeReport>> FetchIncomeReports(Guid itemId)
        {
            var queryStrings = new Dictionary<string, string>
            {
                { "itemId", itemId.ToString() }
            };
            return await httpService.GetAsync<PageResults<IncomeReport>>(URL_INCOME_REPORT, null, queryStrings);
        }

        /// <summary>
        /// Fetch the list of investment transactions
        /// </summary>
        /// <param name="id">Investment Id</param>
        /// <returns>InvestmentTrasnsactions results list</returns>
        public async Task<PageResults<InvestmentTransaction>> FetchInvestmentTransactions(Guid id, TransactionParameters pageParams = null)
        {
            var queryStrings = pageParams != null ? pageParams.ToQueryStrings() : new Dictionary<string, string>();
            return await httpService.GetAsync<PageResults<InvestmentTransaction>>(URL_INVESTMENT_TRANSACTIONS, HTTP.Utils.GetSegment(id.ToString()), queryStrings);
        }

        /// <summary>
        /// Creates a "ConnectToken" that provides an "AccessToken" for client-side communication.
        /// </summary>
        /// <returns>An object containing an accessToken</returns>
        public async Task<ConnectTokenResponse> CreateConnectToken(Guid? itemId = null, ItemOptions options = null)
        {
            try
            {
                var body = new Dictionary<string, object>
                {
                    { "itemId", itemId?.ToString() },
                    { "options", options }
                }.RemoveNulls();
                return await httpService.PostAsync<ConnectTokenResponse>(URL_CONNECT_TOKEN, body);
            }
            catch (ApiException e)
            {
                if (e.ApiError != null && e.ApiError.Errors != null)
                    throw new ValidationException(e.StatusCode, e.ApiError);

                throw e;
            }
        }

        
        /*
         * Execution Helpers
         */

        /// <summary>
        /// Creates a new item for a connector and starts reviewing the status until completition
        /// </summary>
        /// <param name="request">The item parameters</param>
        /// <returns>an object with the info to retrieve the data when the execution is ready</returns>
        public async Task<Item> ExecuteAndWait(ItemParameters request)
        {
            try
            {
                Item item = await CreateItem(request);

                do
                {
                    await Task.Delay(STATUS_POLL_INTERVAL);
                    item = await FetchItem(item.Id);
                }
                while (!item.HasFinished());

                return item;
            }
            catch (ApiException e)
            {
                if (e.ApiError != null && e.ApiError.Errors != null)
                    throw new ValidationException(e.StatusCode, e.ApiError);

                throw e;
            }
        }
    }
}
