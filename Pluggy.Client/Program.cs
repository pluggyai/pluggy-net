using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Pluggy.SDK;
using Pluggy.SDK.Errors;
using Pluggy.SDK.Model;

namespace Pluggy.Client
{
    class Program
    {
        /// <summary>
        /// This application is intended to explain a basic flow of the Pluggy API
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            // Fetching keys from appsettings.json
            var (CLIENT_ID, CLIENT_SECRET, URL_BASE) = Configuration();

            var sdk = new PluggyAPI(CLIENT_ID, CLIENT_SECRET, URL_BASE);

            // 1 - Let's list all available connectors
            var reqParams = new ConnectorParameters
            {
                Countries = new List<string> { "AR", "BR" },
                Types = new List<string> { "PERSONAL_BANK", "BUSINESS_BANK", "INVESTMENT" },
                Name = "",
                Sandbox = true
            };
            var connectors = await sdk.FetchConnectors(reqParams);
            WriteConnectorList(connectors.Results);

            // 2 - Select a connector
            Console.WriteLine("Which connector do you want to execute?");
            string connectorNumberResponse = Console.ReadLine();
            long connectorId = long.Parse(connectorNumberResponse);

            // Fetch that connector and display
            Connector connector = await FetchConnector(sdk, connectorId);
            if (connector == null) return;

            Console.WriteLine("Executing {0}", connector.Name);

            // 3 - Ask for credentials to execute this connector
            ItemParameters request = AskCredentials(connector);

            // 4 - Starts & retrieves the item metadata
            Console.WriteLine("Starting your connection based on the information provided");
            DateTime started = DateTime.Now;
            Item item = await CreateItem(sdk, request);

            if (item == null) return;
            Console.WriteLine("Connection to Item {0} started", item.Id);


            // 5 - Reviews connection status and collects response
            item = await WaitAndCollectResponse(sdk, item);
            Console.WriteLine("Connection has been completed");

            if (item.Error != null)
            {
                Console.WriteLine("Connection encoutered errors, {0}", item.Error.Message);
                return;
            }
            else
            {
                Console.WriteLine("Connection was completed successfully in {0}s", (DateTime.Now - started).TotalSeconds);
            }

            // 6 - List connected accounts with their transactions
            var accounts = await sdk.FetchAccounts(item.Id);

            foreach (var account in accounts.Results)
            {

                Console.WriteLine("Account # {0}, Number {1} has a balance of ${2}", account.Id, account.Number, account.Balance);
                //var txParams = new TransactionParameters() { DateFrom = "1990-01-01", DateTo = "2020-05-26" };
                var transactions = await sdk.FetchTransactions(account.Id);
                foreach (var tx in transactions.Results)
                {
                    Console.WriteLine("  Transaction # {0} made at {1}, description: {2}, amount: {3}", tx.Id, tx.Date.ToLongDateString(), tx.Description, tx.Amount);
                }
            }

            // 7 - Review the identity of the user
            var identity = await sdk.FetchIdentityByItemId(item.Id);
            Console.WriteLine("The name of the user is {0} and his email is {1}.", identity.FullName, identity.Emails.First().Value);

            // 8 - If needed, delete the connection result from the cache.
            Console.WriteLine("Do you want to delete the Connection? (y/n)");
            bool delete = Console.ReadLine() == "y";
            if (delete)
            {
                // Although this will be deleted in 30', we are forcing clean up
                await sdk.DeleteItem(item.Id);
                Console.WriteLine("Deleted response successfully");
            }

            Console.WriteLine("Explore our SDK to know what else you can do!");
            Console.ReadLine();

        }

        private static (string, string, string) Configuration()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
            return (config["CLIENT_ID"], config["CLIENT_SECRET"], config["URL_BASE"]);
        }

        /// <summary>
        /// Once the execution has been submited correctly,
        /// Poll for the execution status, in an interval
        /// </summary>
        /// <param name="sdk"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private static async Task<Item> WaitAndCollectResponse(PluggyAPI sdk, Item item)
        {
            Item itemResponse;

            do
            {
                await Task.Delay(PluggyAPI.STATUS_POLL_INTERVAL);
                Console.WriteLine("Checking Connection status...");
                itemResponse = await sdk.FetchItem(item.Id);
            }
            while (!itemResponse.HasFinished());

            return itemResponse;
        }





        #region Execution Steps

        /// <summary>
        /// Using the SDK we retrieve a connector by its Id
        /// Different exceptions are provided to use on the flow
        /// </summary>
        /// <param name="sdk"></param>
        /// <param name="connectorId"></param>
        /// <returns></returns>
        private static async Task<Connector> FetchConnector(PluggyAPI sdk, long connectorId)
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
        private static ItemParameters AskCredentials(Connector connector)
        {
            Console.WriteLine("Please provide the following credentials to retrieve the data");
            List<ItemParameter> credentials = new List<ItemParameter>();
            foreach (var credential in connector.Credentials)
            {
                Console.WriteLine("What is your {0}?", credential.Label);
                string response = Console.ReadLine();
                credentials.Add(new ItemParameter(credential.Name, response));
            }

            // Create an execution request to retrieve the last 15 days of transactions
            return new ItemParameters(connector.Id, credentials);
        }

        /// <summary>
        /// Starts the execution of a Connector based on the parameters
        /// </summary>
        /// <param name="sdk"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private static async Task<Item> CreateItem(PluggyAPI sdk, ItemParameters request)
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

        #endregion



        #region Helpers
        static void WriteJson(object data)
        {
            Console.WriteLine("JSON Response: ");
            Console.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        static void WriteConnectorList(IList<Connector> connectors)
        {
            foreach (var connector in connectors)
            {
                Console.WriteLine("[{0}] Connector for {1}.", connector.Id.ToString("000"), connector.Name);
            }
        }

        static void WriteOptionalRequests()
        {
            Console.WriteLine("001 - Fetch Accounts");
        }
    }
    #endregion
}
