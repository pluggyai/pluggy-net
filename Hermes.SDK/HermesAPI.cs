using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hermes.SDK.HTTP;
using Hermes.SDK.Model;

namespace Hermes.SDK
{
    public class HermesAPI
    {
        protected readonly APIService httpService;
        protected static readonly string URL_ROBOT = "/robots";
        protected static readonly string URL_EXECUTION = "/executions";

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
            ApiResponse<List<Robot>> apiResponse = await httpService.GetAsync<ApiResponse<List<Robot>>>(URL_ROBOT,
                                                                                                        null,
                                                                                                        null,
                                                                                                        null);
            if (!apiResponse.Ok)
            {
                throw new APIServiceException();
            }
            return apiResponse.Data;
        }

        /// <summary>
        /// Fetch a single robot
        /// </summary>
        /// <param name="id">The robot ID</param>
        /// <returns>A robot object</returns>
        public async Task<Robot> FetchRobot(long id)
        {
            ApiResponse<Robot> apiResponse = await httpService.GetAsync<ApiResponse<Robot>>(URL_ROBOT + "/{id}",
                                                                                            Utils.GetSegment(id.ToString()),
                                                                                            null,
                                                                                            null);
            if (!apiResponse.Ok)
            {
                throw new APIServiceException();
            }
            return apiResponse.Data;
        }

        /// <summary>
        /// Creates a new execution of a robot
        /// </summary>
        /// <param name="robotId">the ID of the robot to be executed</param>
        /// <param name="request">The executions parameters</param>
        /// <returns>an object with the info to retrieve the data when the execution is ready</returns>
        public async Task<Execution> Execute(long robotId, ExecutionParameters request)
        {
            ApiResponse<Execution> apiResponse = await httpService.PostAsync<ApiResponse<Execution>>(URL_EXECUTION + "/{robotId}",
                                                                                                     request,
                                                                                                     null,
                                                                                                     Utils.GetSegment(robotId.ToString(), "robotId"),
                                                                                                     null,
                                                                                                     null);
            if (!apiResponse.Ok)
            {
                throw new APIServiceException();
            }
            return apiResponse.Data;
        }

        /// <summary>
        /// Fetch a single execution
        /// </summary>
        /// <param name="id">Execution id</param>
        /// <returns></returns>
        public async Task<ExecutionResponse> FetchExecution(Guid id)
        {
            ApiResponse<ExecutionResponse> apiResponse = await httpService.GetAsync<ApiResponse<ExecutionResponse>>(URL_EXECUTION + "/{id}",
                                                                                                                    Utils.GetSegment(id.ToString()),
                                                                                                                    null,
                                                                                                                    null);
            if (!apiResponse.Ok)
            {
                throw new APIServiceException();
            }
            return apiResponse.Data;
        }

        /// <summary>
        /// Deletes an execution by the execution id
        /// </summary>
        /// <param name="id">Execution id</param>
        /// <returns></returns>
        public async Task DeleteExecution(Guid id)
        {
            ApiResponse<object> apiResponse = await httpService.DeleteAsync<ApiResponse<object>>(URL_EXECUTION + "/{id}",
                                                                                                 null,
                                                                                                 Utils.GetSegment(id.ToString()),
                                                                                                 null);
            if (!apiResponse.Ok)
            {
                throw new APIServiceException();
            }
        }
    } 
}
