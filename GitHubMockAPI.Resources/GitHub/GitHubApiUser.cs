namespace GitHubMockAPI.Resources.GitHub
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Defines the <see cref="GitHubApiUser" />.
    /// </summary>
    public class GitHubApiUser
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the Login.
        /// </summary>
        [JsonPropertyName("login")]
        public string? Login { get; set; }

        /// <summary>
        /// Gets or sets the Company.
        /// </summary>
        public string? Company { get; set; }

        /// <summary>
        /// Gets or sets the Followers.
        /// </summary>
        [JsonPropertyName("followers")]
        public int Followers { get; set; }

        /// <summary>
        /// Gets or sets the PublicRepos.
        /// </summary>
        [JsonPropertyName("public_repos")]
        public int PublicRepos { get; set; }
    }
}
