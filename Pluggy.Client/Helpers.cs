using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pluggy.SDK;
using Pluggy.SDK.Errors;
using Pluggy.SDK.Model;

namespace Pluggy.Client
{
    public static class Helpers
    {
        public static void WriteJson(object data)
        {
            Console.WriteLine("JSON Response: ");
            Console.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        public static void WriteConnectorList(IList<Connector> connectors)
        {
            foreach (var connector in connectors)
            {
                Console.WriteLine("[{0}] Connector for {1}.", connector.Id.ToString("000"), connector.Name);
            }
        }

        public static void WriteOptionalRequests()
        {
            Console.WriteLine("001 - Fetch Accounts");
        }

        /// <summary>
        /// Once the execution has been submited correctly,
        /// Poll for the execution status, in an interval
        /// </summary>
        /// <param name="sdk"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static async Task<Item> WaitAndCollectResponse(PluggyAPI sdk, Item item)
        {
            Item itemResponse;

            do
            {
                await Task.Delay(PluggyAPI.STATUS_POLL_INTERVAL);
                Console.WriteLine("Checking Connection status...");
                itemResponse = await sdk.FetchItem(item.Id);

                // For MFA connections, we require to provide an extra credential
                if (itemResponse.Status == ItemStatus.WAITING_USER_INPUT)
                {
                    var credential = itemResponse.Parameter;
                    Console.WriteLine("What is your {0}?", credential.Label);
                    string response = Console.ReadLine();
                    var parameter = new ItemParameter(credential.Name, response);
                    itemResponse = await sdk.UpdateItemMFA(item.Id, new List<ItemParameter> { parameter });
                }
            }
            while (!itemResponse.HasFinished());

            return itemResponse;
        }

        /// <summary>
        /// Using the SDK we retrieve a connector by its Id
        /// Different exceptions are provided to use on the flow
        /// </summary>
        /// <param name="sdk"></param>
        /// <param name="connectorId"></param>
        /// <returns></returns>
        public static async Task<Connector> FetchConnector(PluggyAPI sdk, long connectorId)
        {
            try
            {
                return await sdk.FetchConnector(connectorId);
            }
            catch (NotFoundException)
            {
                Console.WriteLine("The connector with Id {0} was not found.", connectorId);
                Console.ReadLine();
                return null;
            }
            catch (ApiException e)
            {
                Console.WriteLine("There was an issue retrieving connection");
                Console.WriteLine(e.Message);
                Console.ReadLine();
                return null;
            }
        }


        /// <summary>
        /// Iterates over each parameter needed to execute the Connector.
        /// Parameters have the information required to build Forms.
        /// </summary>
        /// <param name="connector">Connector information</param>
        /// <returns>An execution request</returns>
        public static ItemParameters AskCredentials(long connectorId, IList<ConnectorParameter> requestedCredentials)
        {
            Console.WriteLine("Please provide the following credentials to retrieve the data");
            List<ItemParameter> credentials = new List<ItemParameter>();
            foreach (var credential in requestedCredentials)
            {
                Console.WriteLine("What is your {0}?", credential.Label);
                string response = Console.ReadLine();
                credentials.Add(new ItemParameter(credential.Name, response));
            }

            // Create an execution request to retrieve the last 15 days of transactions
            return new ItemParameters(connectorId, credentials);
        }


        /// <summary>
        /// Starts the execution of a Connector based on the parameters
        /// </summary>
        /// <param name="sdk"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<Item> CreateItem(PluggyAPI sdk, ItemParameters request)
        {
            try
            {
                return await sdk.CreateItem(request);
            }
            catch (ValidationException e)
            {
                Console.WriteLine("Execution not started, reason {0} ", e.Message);
                if (e.ApiError.Errors != null && e.ApiError.Errors.Count > 0)
                {
                    foreach (var error in e.ApiError.Errors)
                    {
                        Console.WriteLine("[X] {0} ", error.Message);
                    }
                }
                Console.ReadLine();
                return null;
            }
            catch (ApiException e)
            {
                Console.WriteLine("There was an issue starting the execution");
                Console.WriteLine(e.Message);
                Console.ReadLine();
                return null;
            }
        }

        public static async Task PrintResults(SDK.PluggyAPI sdk, Item item)
        {
            // List connected products
            var accounts = await sdk.FetchAccounts(item.Id);

            foreach (var account in accounts.Results)
            {
                Console.WriteLine("Account # {0}, Number {1} has a balance of ${2}", account.Id, account.Number, account.Balance);
                var txSearchParams = new TransactionParameters() { DateFrom = DateTime.Now.AddYears(-1), DateTo = DateTime.Now };
                var transactions = await sdk.FetchTransactions(account.Id, txSearchParams);
                foreach (var tx in transactions.Results)
                {
                    Console.WriteLine("  Transaction # {0} made at {1}, description: {2}, amount: {3}", tx.Id, tx.Date.ToLongDateString(), tx.Description, tx.Amount);
                }
            }

            var investments = await sdk.FetchInvestments(item.Id);
            foreach (var investment in investments.Results)
            {
                Console.WriteLine("Investment #{0}, Code {1} has a balance of ${2}", investment.Id, investment.Code, investment.Balance);
                Helpers.WriteJson(investment);
            }

            var identity = await sdk.FetchIdentityByItemId(item.Id);
            Console.WriteLine("The name of the user is {0}", identity.FullName);
        }

    }
}
