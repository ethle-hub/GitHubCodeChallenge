namespace GitHubMockAPI.Resources.GitHub
{
    using Common.HttpClient;
    using Common.Entities;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines a caller to GitHub Api, <see cref="GitHubApiClient" />.
    /// </summary>
    public class GitHubApiClient : HttpClientBase, IGitHubApi
    {
        /// <summary>
        /// Defines the _httpClient.
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Defines the _logger.
        /// </summary>
        private readonly ILogger<GitHubApiClient> _logger;

        /// <summary>
        /// Defines the _gitHubApiOptions.
        /// </summary>
        private readonly GitHubApiOptions _gitHubApiOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubApiClient"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger{GitHubApiClient}"/>.</param>
        /// <param name="httpClient">The httpClient<see cref="HttpClient"/>.</param>
        /// <param name="gitHubApiOptions">The gitHubApiOptions<see cref="GitHubApiOptions"/>.</param>
        public GitHubApiClient(ILogger<GitHubApiClient> logger, HttpClient httpClient, GitHubApiOptions gitHubApiOptions)
        {
            _logger = logger;
            _httpClient = httpClient;
            _gitHubApiOptions = gitHubApiOptions;
        }

        /// <summary>
        /// The GetUser.
        /// </summary>
        /// <param name="username">The username<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{ServiceResult{GitHubApiUser}}"/>.</returns>
        public async Task<ServiceResult<GitHubApiUser>> GetUser(string username)
        {
            try
            {
                _logger.LogDebug($"calling {nameof(GitHubApiClient)}:{nameof(GetUser)}");
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_gitHubApiOptions.BaseUrl}/users/{username}");    // e.g. https://api.github.com/users/{username}
                
                // notes: GitHubAPI expected a header from the request
                foreach (var customHeader in _gitHubApiOptions.Headers
                    .Where(hr => !string.IsNullOrWhiteSpace(hr.Key) && !string.IsNullOrWhiteSpace(hr.Value)))
                {
                    request.Headers.UserAgent.Add(new ProductInfoHeaderValue(customHeader.Key, customHeader.Value));
                }

                var httpResponse = await _httpClient.SendAsync(request);

                return await ConvertToServiceResult<GitHubApiUser>(httpResponse);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {nameof(ErrorType.DependencyError)} >>> {e.Message}");
                return ServiceOutcome.Error(ErrorType.DependencyError, e.Message);
            }
        }
    }
}
