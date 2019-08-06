using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hermes.SDK;
using Hermes.SDK.Model;
using Newtonsoft.Json;

namespace Hermes.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var sdk = new HermesAPI("MY_API_KEY", "https://hermes.free.beeceptor.com");

            // Let's list all available robots
            var robots = await sdk.FetchRobots();
            WriteJson(robots);

            // Select a robot
            Console.WriteLine("Which robot do you want to execute?");
            string robotNumberResponse = Console.ReadLine();
            long robotId = long.Parse(robotNumberResponse);

            // Fetch that robot and display
            var robot = await sdk.FetchRobot(robotId);
            WriteJson(robot);
            Console.WriteLine("Executing {0}", robot.Name); 

            // Ask for credentials to execute this robot
            Console.WriteLine("Please provide the following credentials to retrieve the data");
            List<ExecuteParameter> credentials = new List<ExecuteParameter>();
            foreach (var credential in robot.Credentials)
            {
                Console.WriteLine("What is your {0}?", credential.Label);
                string response = Console.ReadLine();
                credentials.Add(new ExecuteParameter(credential.Name, response));
            }

            // Create an execution request to retrieve the last 15 days of transactions
            ExecutionParameters request = new ExecutionParameters(credentials);
            request.StartDate = DateTime.Now.AddDays(-15);
            request.EndDate = DateTime.Now;

            // Starts & retrieves the execution metadata
            Console.WriteLine("Starting your execution based on the information provided");
            Execution execution = await sdk.Execute(robotId, request);
            Console.WriteLine("Execution {0} started", execution.Id);

            ExecutionResponse executionResponse;
            do
            {
                // Fetch the execution
                Console.WriteLine("Checking execution status");
                executionResponse = await sdk.FetchExecution(execution.Id);
                WriteJson(executionResponse);
            }
            while (!executionResponse.Finished);

            Console.WriteLine("Execution has been completed");


            Console.WriteLine("Do you want to delete the results? (y/n)");
            string delete = Console.ReadLine();
            if (delete == "y")
            {
                // Although this will be deleted in 30', we are forcing clean up
                await sdk.DeleteExecution(execution.Id);
                Console.WriteLine("Deleted response");
            }
        }


        static void WriteJson(object data)
        {
            Console.WriteLine("JSON Response: ");
            Console.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
        }
    }
}
