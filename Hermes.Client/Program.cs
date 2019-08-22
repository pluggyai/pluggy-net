using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hermes.SDK;
using Hermes.SDK.Model;
using Newtonsoft.Json;
using Hermes.SDK.Errors;

namespace Hermes.Client
{
    class Program
    {
        /// <summary>
        /// This application is intended to explain a basic flow of the Hermes API
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            var sdk = new HermesAPI("MY_API_KEY");

            // 1 - Let's list all available robots
            var robots = await sdk.FetchRobots();
            WriteRobotList(robots);

            // 2 - Select a robot
            Console.WriteLine("Which robot do you want to execute?");
            string robotNumberResponse = Console.ReadLine();
            long robotId = long.Parse(robotNumberResponse);

            // Fetch that robot and display
            Robot robot = await FetchRobot(sdk, robotId);
            if (robot == null) return;

            Console.WriteLine("Executing {0}", robot.Name);

            // 3 - Ask for credentials to execute this robot
            ExecutionParameters request = AskCredentials(robot);

            // 4 - Starts & retrieves the execution metadata
            Console.WriteLine("Starting your execution based on the information provided");
            Execution execution = await StartExecution(sdk, robot, request);
            if (execution == null) return;
            Console.WriteLine("Execution {0} started", execution.Id);


            // 5 - Reviews execution status and collects response
            ExecutionResponse response = await WaitAndCollectResponse(sdk, execution);
            Console.WriteLine("Execution has been completed");

            if (response.Error != null)
            {
                Console.WriteLine("Execution encoutered errors, {0}", response.Error.Message);
            }
            else
            {
                Console.WriteLine("Execution was completed successfully in {0}s",
                    (response.EndTime.Value - response.StartTime).TotalSeconds);
                WriteJson(response.Data);
            }

            // 6 - If needed, delete the execution result from the cache.
            Console.WriteLine("Do you want to delete the results? (y/n)");
            bool delete = Console.ReadLine() == "y";
            if (delete)
            {
                // Although this will be deleted in 30', we are forcing clean up
                await sdk.DeleteExecution(execution.Id);
                Console.WriteLine("Deleted response successfully");
            }
        }

        /// <summary>
        /// Once the execution has been submited correctly,
        /// Poll for the execution status, in an interval
        /// </summary>
        /// <param name="sdk"></param>
        /// <param name="execution"></param>
        /// <returns></returns>
        private static async Task<ExecutionResponse> WaitAndCollectResponse(HermesAPI sdk, Execution execution)
        {
            ExecutionResponse executionResponse;

            do
            {
                Console.WriteLine("Checking execution status");
                executionResponse = await sdk.FetchExecution(execution.Id);
                if (!executionResponse.Finished)
                {
                    Console.WriteLine("Execution has not finished, will check again.");
                    await Task.Delay(HermesAPI.STATUS_POLL_INTERVAL);
                }
            }
            while (!executionResponse.Finished);

            return executionResponse;
        }





        #region Execution Steps

        /// <summary>
        /// Using the SDK we retrieve a robot by its Id
        /// Different exceptions are provided to use on the flow
        /// </summary>
        /// <param name="sdk"></param>
        /// <param name="robotId"></param>
        /// <returns></returns>
        private static async Task<Robot> FetchRobot(HermesAPI sdk, long robotId)
        {
            try
            {
                return await sdk.FetchRobot(robotId);
            }
            catch (NotFoundException)
            {
                Console.WriteLine("The connector with Id {0} was not found.", robotId);
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
        /// <param name="robot">Robot information</param>
        /// <returns>An execution request</returns>
        private static ExecutionParameters AskCredentials(Robot robot)
        {
            Console.WriteLine("Please provide the following credentials to retrieve the data");
            List<ExecuteParameter> credentials = new List<ExecuteParameter>();
            foreach (var credential in robot.Credentials)
            {
                Console.WriteLine("What is your {0}?", credential.Label);
                string response = Console.ReadLine();
                credentials.Add(new ExecuteParameter(credential.Name, response));
            }

            // Create an execution request to retrieve the last 15 days of transactions
            return new ExecutionParameters(credentials, DateTime.Now.AddDays(-15), DateTime.Now);
        }

        /// <summary>
        /// Starts the execution of a Connector based on the parameters
        /// </summary>
        /// <param name="sdk"></param>
        /// <param name="robot"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private static async Task<Execution> StartExecution(HermesAPI sdk, Robot robot, ExecutionParameters request)
        {
            try
            {
                return await sdk.Execute(robot.Id, request);
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

        static void WriteRobotList(IList<Robot> robots)
        {
            foreach(var robot in robots)
            {
                Console.WriteLine("[{0}] Robot for {1}.", robot.Id, robot.Name);
            }
        }
        #endregion
    }
}
