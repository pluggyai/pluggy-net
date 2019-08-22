using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hermes.SDK.Errors;
using Hermes.SDK.HTTP;
using Hermes.SDK.Model;

namespace Hermes.SDK
{
    public class HermesAPI
    {
        protected readonly APIService httpService;

        protected static readonly string URL_ROBOT = "/robots";
        protected static readonly string URL_EXECUTION = "/executions";
        protected static readonly string URL_VALIDATE = "/validations";

        public static readonly int STATUS_POLL_INTERVAL = 3000;

        public HermesAPI(string _apiKey, string _baseUrl = "https://api.hermesapi.com/v1")
        {
            httpService = new APIService(_apiKey, _baseUrl);
        }

        /// <summary>
        /// Fetch all available robots from Hermes API
        /// </summary>
        /// <returns>An array of robots</returns>
        public async Task<List<Robot>> FetchRobots()
        {
            return await httpService.GetAsync<List<Robot>>(URL_ROBOT);
        }

        /// <summary>
        /// Fetch a single robot
        /// </summary>
        /// <param name="id">The robot ID</param>
        /// <returns>A robot object</returns>
        public async Task<Robot> FetchRobot(long id)
        {
            return await httpService.GetAsync<Robot>(URL_ROBOT + "/{id}", Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Creates a new execution of a robot
        /// </summary>
        /// <param name="robotId">the ID of the robot to be executed</param>
        /// <param name="request">The executions parameters</param>
        /// <returns>an object with the info to retrieve the data when the execution is ready</returns>
        public async Task<Execution> Execute(long robotId, ExecutionParameters request)
        {
            try
            {
                return await httpService.PostAsync<Execution>(URL_EXECUTION, request.ToBody(), null, null, null,
                    Utils.GetSegment(robotId.ToString(), "robot_id"));

            }
            catch (ApiException e)
            {
                if (e.ApiError != null && e.ApiError.Errors != null)
                    throw new ValidationException(e.StatusCode, e.ApiError);

                throw e;
            }
        }


        /// <summary>
        /// Creates a new execution of a robot, and polls for status until
        /// it recovers the final response (Error or Result).
        /// </summary>
        /// <param name="robotId">the ID of the robot to be executed</param>
        /// <param name="request">The executions parameters</param>
        /// <returns>an object with the info to retrieve the data when the execution is ready</returns>
        public async Task<ExecutionResponse> ExecuteAndWait(long robotId, ExecutionParameters request)
        {
            try
            {
                Execution execution = await httpService.PostAsync<Execution>(URL_EXECUTION, request.ToBody(), null, null, null,
                    Utils.GetSegment(robotId.ToString(), "robot_id"));

                ExecutionResponse executionResponse;

                do
                {
                    executionResponse = await FetchExecution(execution.Id);
                    if (!executionResponse.Finished)
                    {
                        await Task.Delay(STATUS_POLL_INTERVAL);
                    }
                }
                while (!executionResponse.Finished);

                return executionResponse;

            }
            catch (ApiException e)
            {
                if (e.ApiError != null && e.ApiError.Errors != null)
                    throw new ValidationException(e.StatusCode, e.ApiError);

                throw e;
            }
        }

        /// <summary>
        /// Fetch a single execution
        /// </summary>
        /// <param name="id">Execution id</param>
        /// <returns></returns>
        public async Task<ExecutionResponse> FetchExecution(Guid id)
        {
            return await httpService.GetAsync<ExecutionResponse>(URL_EXECUTION + "/{id}", Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Deletes an execution by the execution id
        /// </summary>
        /// <param name="id">Execution id</param>
        /// <returns></returns>
        public async Task DeleteExecution(Guid id)
        {
            await httpService.DeleteAsync<dynamic>(URL_EXECUTION + "/{id}", null, Utils.GetSegment(id.ToString()));
        }

        /// <summary>
        /// Validate the executions parameters for a specific robot
        /// </summary>
        /// <param name="robotId">the ID of the robot to be executed</param>
        /// <param name="request">The executions parameters</param>
        /// <returns>an object with the info to retrieve the data when the execution is ready</returns>
        public async Task<Execution> Validate(long robotId, ExecutionParameters request)
        {
            try
            {
                return await httpService.PostAsync<Execution>(URL_VALIDATE + "/{robotId}",
                                                            request,
                                                            null,
                                                            Utils.GetSegment(robotId.ToString(), "robotId"));
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
