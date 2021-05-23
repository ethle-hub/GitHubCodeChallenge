namespace GitHubMockAPI.Services.Implementations
{
    using Contracts;
    using Common.Entities;
    using Interfaces;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Resources.GitHub;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// This <see cref="UsersFromGitHubService" /> class implements `IUsersService` interface for Users service.
    /// </summary>
    public class UsersFromGitHubService : IUsersService
    {
        /// <summary>
        /// Defines the _defaultCacheExpirationInMinute.
        /// </summary>
        private int _defaultCacheExpirationInMinute = 2;

        /// <summary>
        /// Defines the _logger.
        /// </summary>
        private readonly ILogger<UsersFromGitHubService> _logger;

        /// <summary>
        /// Defines the _gitHubApi.
        /// </summary>
        private readonly IGitHubApi _gitHubApi;

        /// <summary>
        /// Defines the _cache.
        /// </summary>
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Defines the _serviceOptions.
        /// </summary>
        private readonly UsersFromGitHubServiceOptions _serviceOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersFromGitHubService"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger{GitHubUsers}"/>.</param>
        /// <param name="gitHubApi">The gitHubApi<see cref="IGitHubApi"/>.</param>
        /// <param name="cache">.</param>
        /// <param name="serviceOptions">.</param>
        public UsersFromGitHubService(ILogger<UsersFromGitHubService> logger, IGitHubApi gitHubApi, IMemoryCache cache, UsersFromGitHubServiceOptions serviceOptions)
        {
            _logger = logger;
            _gitHubApi = gitHubApi;
            _cache = cache;
            _serviceOptions = serviceOptions;
        }

        /// <summary>
        /// The FindUsersAsync.
        /// </summary>
        /// <param name="usernames">The usernames<see cref="string[]"/>.</param>
        /// <returns>The <see cref="Task{ServiceResult{User}}"/>.</returns>
        public async Task<ServiceResult<List<User>>> FindUsersAsync(params string[] usernames)
        {
            try
            {
                _logger.LogDebug($"executing {nameof(UsersFromGitHubService)}:{nameof(FindUsersAsync)}");

                var validUsernames = usernames.Where(name => !string.IsNullOrWhiteSpace(name));

                var gitHubUsers = new List<GitHubApiUser>();
                foreach (var username in validUsernames)
                {
                    // Look for user in cache.
                    var userInCacheResult = GetUserFromCache(username);
                    if (userInCacheResult.Outcome)
                    {
                        gitHubUsers.Add(userInCacheResult.Data);
                    }
                    else
                    {
                        // User is not in cache, so get from GitHub
                        var response = await _gitHubApi.GetUser(username);
                        if (!response.Outcome)
                            continue;

                        gitHubUsers.Add(response.Data);

                        var addGetUserToCacheResult = AddGetUserToCache(username, response.Data);
                        if (!addGetUserToCacheResult.Outcome)
                        {
                            _logger.LogWarning($"{addGetUserToCacheResult.Outcome.ErrorType} - {addGetUserToCacheResult.Outcome.ErrorMessage}");
                        }
                    }
                }

                return gitHubUsers.Select(ConvertToUser).OrderBy(ur => ur.Name).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {nameof(ErrorType.BusinessError)} >>> {e.Message}");
                return ServiceOutcome.Error(ErrorType.BusinessError, e.Message);
            }
        }

        /// <summary>
        /// The GetUserFromCache.
        /// </summary>
        /// <param name="cacheKey">The cacheKey<see cref="string"/>.</param>
        /// <returns>The <see cref="ServiceResult{GitHubUser}"/>.</returns>
        internal ServiceResult<GitHubApiUser> GetUserFromCache(string cacheKey)
        {
            if (_cache.TryGetValue(key: cacheKey, out GitHubApiUser user))
                return user;
            return ServiceOutcome.Error(ErrorType.DependencyError, $"Cache item {cacheKey} not found");
        }

        /// <summary>
        /// The AddGetUserToCache.
        /// </summary>
        /// <param name="cacheKey">The cacheKey<see cref="string"/>.</param>
        /// <param name="user">The user<see cref="GitHubApiUser"/>.</param>
        /// <returns>The <see cref="Task{ServiceResult{GitHubApiUser}}"/>.</returns>
        internal ServiceResult<GitHubApiUser> AddGetUserToCache(string cacheKey, GitHubApiUser user)
        {
            try
            {
                var cacheExpirationInMinute = (_serviceOptions.CacheExpirationInMinute <= 0)
                    ? _defaultCacheExpirationInMinute
                    : _serviceOptions.CacheExpirationInMinute;
                return _cache.Set(key: cacheKey, user, TimeSpan.FromMinutes(cacheExpirationInMinute));
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Exception: {e.Message}");
                return ServiceOutcome.Error(ErrorType.DependencyError, e.Message);
            }
        }

        /// <summary>
        /// The ConvertToUser.
        /// </summary>
        /// <param name="gitUser">The gitUser<see cref="GitHubApiUser"/>.</param>
        /// <returns>The <see cref="User"/>.</returns>
        internal static User ConvertToUser(GitHubApiUser gitUser)
        {
            return new()
            {
                Company = gitUser.Company,
                Followers = gitUser.Followers,
                Login = gitUser.Login,
                Name = gitUser.Name,
                PublicRepos = gitUser.PublicRepos
            };
        }
    }
}
