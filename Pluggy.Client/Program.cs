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

            Console.WriteLine("Please select an operation to walkthrough");
            Console.WriteLine("1. Create an Item");
            Console.WriteLine("2. Update an Item");
            Console.WriteLine("3. Create a ConnectToekn for Widget");

            string action = Console.ReadLine();

            switch (action)
            {
                case "1":
                    await CreateItem(sdk);
                    break;
                case "2":
                    await UpdateItem(sdk);
                    break;
                case "3":
                    await CreateConnectToken(sdk);
                    break;
            }

            Console.WriteLine("Explore our SDK to know what else you can do!");
            Console.ReadLine();

        }

        /// <summary>
        /// This is a walkthrough on how to create an item (connection) first time.
        /// </summary>
        /// <param name="sdk">Pluggy's api client</param>
        /// <returns></returns>
        private static async Task CreateItem(SDK.PluggyAPI sdk)
        {
            // 1 - Let's list all available connectors
            var reqParams = new ConnectorParameters
            {
                Countries = new List<string> { "AR", "BR" },
                Types = new List<ConnectorType> {
                    ConnectorType.PERSONAL_BANK,
                    ConnectorType.BUSINESS_BANK,
                    ConnectorType.INVESTMENT
                },
                Name = "",
                Sandbox = true
            };
            var connectors = await sdk.FetchConnectors(reqParams);
            Helpers.WriteConnectorList(connectors.Results);

            // 2 - Select a connector
            Console.WriteLine("Which connector do you want to execute?");
            string connectorNumberResponse = Console.ReadLine();
            long connectorId = long.Parse(connectorNumberResponse);

            // Fetch that connector and display
            Connector connector = await Helpers.FetchConnector(sdk, connectorId);
            if (connector == null) return;

            Console.WriteLine("Executing {0}", connector.Name);

            // 3 - Ask for credentials to execute this connector
            ItemParameters request = Helpers.AskCredentials(connector.Id, connector.Credentials);

            // 4 - Starts & retrieves the item metadata
            Console.WriteLine("Starting your connection based on the information provided");
            DateTime started = DateTime.Now;
            Item item = await Helpers.CreateItem(sdk, request);

            if (item == null) return;
            Console.WriteLine("Connection to Item {0} started", item.Id);


            // 5 - Reviews connection status and collects response
            item = await Helpers.WaitAndCollectResponse(sdk, item);
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

            await Helpers.PrintResults(sdk, item);

            // 8 - If needed, delete the connection result from the cache.
            Console.WriteLine("Do you want to delete the Connection? (y/n)");
            bool delete = Console.ReadLine() == "y";
            if (delete)
            {
                // Although this will be deleted in 30', we are forcing clean up
                await sdk.DeleteItem(item.Id);
                Console.WriteLine("Deleted response successfully");
            }
        }

        /// <summary>
        /// This is a walkthrough on how to update an exiting item (connection).
        /// </summary>
        /// <param name="sdk">Pluggy's api client</param>
        /// <returns></returns>
        private static async Task UpdateItem(SDK.PluggyAPI sdk)
        {
            // 1 - Introduce the item's Id
            Console.WriteLine("Which item do you want to update? (Provide itemId)");
            string itemIdStr = Console.ReadLine();
            if (!Guid.TryParse(itemIdStr, out Guid itemId)) return;

            // 2 - Fetch that item and display
            Item item = await sdk.FetchItem(itemId);
            if (item == null) return;

            Console.WriteLine("Updating {0}", item.Connector.Name);

            // 3 - Ask for mfa credentials, if required, to update this connector
            var credentialsWithMFA = item.Connector.Credentials.Where(c => c.Mfa).ToList();
            ItemParameters request = new ItemParameters();
            if (credentialsWithMFA.Count() > 0)
            {
                request = Helpers.AskCredentials(item.Connector.Id, credentialsWithMFA);
            }

            // 4 - Starts & retrieves the item metadata
            Console.WriteLine("Updating your connection based on the information provided");
            DateTime started = DateTime.Now;
            try
            {
                item = await sdk.UpdateItem(itemId, request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("There was an error triggering the update of the account");
                return;
            }

            // 5 - Reviews connection status and collects response
            item = await Helpers.WaitAndCollectResponse(sdk, item);
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


            // 6 - Show the updated products
            await Helpers.PrintResults(sdk, item);
        }

        /// <summary>
        /// This is a walkthrough on how to create a Connect Token
        /// </summary>
        /// <param name="sdk">Pluggy's api client</param>
        /// <returns></returns>
        private static async Task CreateConnectToken(SDK.PluggyAPI sdk)
        {
            // 1 - Get the ItemId if its an update
            Console.WriteLine("You can use the Connect Token for an new or an existing Connection (item).");
            Console.WriteLine("Do you have an ItemId to Update? (y/n)");
            bool isUpdate = Console.ReadLine().ToLower() == "y";

            Guid? itemIdToUpdate = null;
            if (isUpdate)
            {
                Console.WriteLine("Please provide the itemId you want to update.");
                string itemIdStr = Console.ReadLine();
                if (!Guid.TryParse(itemIdStr, out Guid itemId)) return;
                itemIdToUpdate = itemId;
            }

            var connectTokenOptions = new ItemOptions()
            {
                ClientUserId = "sdk-net"
            };

            ConnectTokenResponse response = await sdk.CreateConnectToken(itemIdToUpdate, connectTokenOptions);
            Console.WriteLine("You can use the following token to create a Connect Widget with it!");
            Console.WriteLine(response.AccessToken);
        }

        private static (string, string, string) Configuration()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
            return (config["CLIENT_ID"], config["CLIENT_SECRET"], config["URL_BASE"]);
        }
    }
}
