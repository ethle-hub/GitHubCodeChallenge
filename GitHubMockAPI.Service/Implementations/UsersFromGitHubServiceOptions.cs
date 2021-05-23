namespace GitHubMockAPI.Services.Implementations
{
    /// <summary>
    /// Defines the <see cref="UsersFromGitHubServiceOptions" /> class for additional configuration settings of the <see cref="UsersFromGitHubService" />
    /// </summary>
    public class UsersFromGitHubServiceOptions
    {
        /// <summary>
        /// Gets or sets the CacheExpirationInMinute.
        /// </summary>
        public int CacheExpirationInMinute { get; set; }
    }
}
