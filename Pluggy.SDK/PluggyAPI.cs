using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pluggy.SDK.Errors;
using Pluggy.SDK.HTTP;
using Pluggy.SDK.Model;

namespace Pluggy.SDK
{
    public class PluggyAPI
    {
        protected readonly APIService httpService;

        protected static readonly string URL_AUTH = "/auth";
        protected static readonly string URL_CONNECTORS = "/connectors";
        protected static readonly string URL_ITEMS = "/items";
        protected static readonly string URL_ACCOUNTS = "/accounts";
        protected static readonly string URL_TRANSACTIONS = "/transactions";

        public string ClientId;
        public string ClientSecret;

        public static readonly int STATUS_POLL_INTERVAL = 3000;

        public PluggyAPI(string _apiKey, string _baseUrl = "https://api.pluggy.ai/")
        {
            httpService = new APIService(_apiKey, _baseUrl);
        }


        /// <summary>
        /// Get a new PluggyAPI instance using clientId and clientSecret
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="_baseUrl"></param>
        /// <returns></returns>
        public static async Task<PluggyAPI> GetClient(string clientId, string clientSecret, string _baseUrl = "https://api.pluggy.ai/")
        {
            var body = new Dictionary<string, string>()
            {
                { "clientId", clientId },
                { "clientSecret", clientSecret }
            };
            var response = await new APIService("", _baseUrl).PostAsync<AuthResponse>(URL_AUTH, body, null, null, null, null);
            return new PluggyAPI(response.ApiKey, _baseUrl);
        }

        /// <summary>
        /// Fetch all available connectors
        /// </summary>
        /// <returns>An array of connectors</returns>
        public async Task<PageResults<Connector>> FetchConnectors()
        {
            return await httpService.GetAsync<PageResults<Connector>>(URL_CONNECTORS);
        }

        /// <summary>
        /// Fetch a single connector
        /// </summary>
        /// <param name="id">The connector ID</param>
        /// <returns>A connector object</returns>
        public async Task<Connector> FetchConnector(long id)
        {
            return await httpService.GetAsync<Connector>(URL_CONNECTORS + "/{id}", Utils.GetSegment(id.ToString()));
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
                return await httpService.PostAsync<Item>(URL_ITEMS, request.ToBody(), null, null, null);
            }
            catch (ApiException e)
            {
                if (e.ApiError != null && e.ApiError.Errors != null)
                    throw new ValidationException(e.StatusCode, e.ApiError);

                throw e;
            }
        }


        /// <summary>
        /// Creates a new item for a connector and starts reviewing the status until completition
        /// </summary>
        /// <param name="request">The item parameters</param>
        /// <returns>an object with the info to retrieve the data when the execution is ready</returns>
        public async Task<Item> ExecuteAndWait(ItemParameters request)
        {
            try
            {
                Item item = await this.CreateItem(request);

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

        /// <summary>
        /// Fetch a single item
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns></returns>
        public async Task<Item> FetchItem(Guid id)
        {
            return await httpService.GetAsync<Item>(URL_ITEMS + "/{id}", Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Deletes an Item by it's primary identifier
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns></returns>
        public async Task DeleteItem(Guid id)
        {
            await httpService.DeleteAsync<dynamic>(URL_ITEMS + "/{id}", Utils.GetSegment(id.ToString()), null);
        }

        /// <summary>
        /// Fetch the list of accounts
        /// </summary>
        /// <param name="id">Item Id</param>
        /// <returns>Account results list</returns>
        public async Task<PageResults<Account>> FetchAccounts(Guid id)
        {
            return await httpService.GetAsync<PageResults<Account>>(URL_ACCOUNTS, null, Utils.GetSegment(id.ToString(), "itemId"));
        }

        /// <summary>
        /// Fetch the list of transactions
        /// </summary>
        /// <param name="id">Account Id</param>
        /// <returns>Transacion results list</returns>
        public async Task<PageResults<Transaction>> FetchTransactions(Guid id)
        {
            return await httpService.GetAsync<PageResults<Transaction>>(URL_TRANSACTIONS, null, Utils.GetSegment(id.ToString(), "accountId"));
        }
    }
}
