namespace GitHubMockAPI.Controllers
{
    using Contracts;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Services;
    using Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Net.Mime;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="UsersController" />.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// Defines the _logger.
        /// </summary>
        private readonly ILogger<UsersController> _logger;

        /// <summary>
        /// Defines the _usersService.
        /// </summary>
        private readonly IUsersService _usersService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger{UsersController}"/>.</param>
        /// <param name="usersService">.</param>
        public UsersController(ILogger<UsersController> logger, IUsersService usersService)
        {
            _logger = logger;
            _usersService = usersService;
        }

        /// <summary>
        /// The Lookup.
        /// </summary>
        /// <param name="request">The request<see cref="ApiRequest{List{string}}"/>.</param>
        /// <returns>The <see cref="Task{ApiResponse{User[]}}"/>.</returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<List<User>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<User>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<List<User>>), StatusCodes.Status500InternalServerError)]
        public async Task<ApiResponse<List<User>>> RetrieveUsers([FromBody] ApiRequest<List<string>> request)
        {
            try
            {
                _logger.LogDebug($"Action {nameof(UsersController)}:{nameof(RetrieveUsers)}");

                if (request?.Data == null)
                {
                    return new ApiResponse<List<User>>
                    {
                        IsSuccess = false,
                        ErrorMessage = "Invalid request"
                    };
                }

                var result = await _usersService.FindUsersAsync(request.Data.ToArray());
                return result.Data.ConvertToApiResponse();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new ApiResponse<List<User>>
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
            }
        }
    }
}
