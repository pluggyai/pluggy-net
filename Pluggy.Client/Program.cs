using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pluggy.SDK;
using Pluggy.SDK.Errors;
using Pluggy.SDK.Model;

namespace Pluggy.Client
{
    class Program
    {

        static string CLIENT_ID = "ccd5b9be-3be6-45fe-9512-5907f1619db0";
        static string CLIENT_SECRET = "5RwpsF9QJUfoNUvkDZoGrmAioZBaAYuwo4KB";

        /// <summary>
        /// This application is intended to explain a basic flow of the Pluggy API
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            var sdk = await PluggyAPI.GetClient(CLIENT_ID, CLIENT_SECRET, "https://dev-pluggy-api.herokuapp.com/");

            // 1 - Let's list all available connectors
            var connectors = await sdk.FetchConnectors();
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
            Item response = await WaitAndCollectResponse(sdk, item);
            Console.WriteLine("Connection has been completed");

            if (response.Error != null)
            {
                Console.WriteLine("Connection encoutered errors, {0}", response.Error.Message);
            }
            else
            {
                Console.WriteLine("Connection was completed successfully in {0}s",
                    (DateTime.Now - started).TotalSeconds);
            }

            // 6 - List connected accounts with their transactions
            var accounts = await sdk.FetchAccounts(item.Id);

            foreach (var account in accounts.Results)
            {
                Console.WriteLine("Account # {0}, Number {1} has a balance of ${2}", account.Id, account.Number, account.Balance);
                var transactions = await sdk.FetchTransactions(account.Id);
                foreach (var tx in transactions.Results)
                {
                    Console.WriteLine("  Transaction # {0} made at {1}, description: {2}, amount: {3}", tx.Id, tx.Date.ToLongDateString(), tx.Description, tx.Amount);
                }
            }

            //// 9 - Expore what else you can do.
            //Console.WriteLine("Expore what else you can do:");
            //WriteOptionalRequests();

            //// 10 - If needed, delete the execution result from the cache.
            //Console.WriteLine("Do you want to delete the Connection? (y/n)");
            //bool delete = Console.ReadLine() == "y";
            //if (delete)
            //{
            //    // Although this will be deleted in 30', we are forcing clean up
            //    await sdk.DeleteItem(item.Id);
            //    Console.WriteLine("Deleted response successfully");
            //}

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
            while (!item.HasFinished());

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
