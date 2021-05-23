namespace GitHubMockAPI.Resources.GitHub
{
    using System.Collections.Generic;

    /// <summary>
    /// The inputs a client needs to connect to the GitHub API.
    /// Usage: to be mapped to the section "Resources:GitHubApi:Options" in appsettings.{env}.json
    /// </summary>
    public class GitHubApiOptions
    {
        /// <summary>
        /// Gets or sets the BaseUrl
        /// Base URL of the service..
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the Headers.
        /// </summary>
        public IDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubApiOptions"/> class.
        /// </summary>
        public GitHubApiOptions()
        {
            BaseUrl = string.Empty;
            Headers = new Dictionary<string, string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubApiOptions"/> class.
        /// </summary>
        /// <param name="baseUrl">The baseUrl<see cref="string"/>.</param>
        /// <param name="headers">.</param>
        public GitHubApiOptions(string baseUrl, Dictionary<string, string>? headers = null)
        {
            BaseUrl = baseUrl;
            Headers = headers ?? new Dictionary<string, string>();
        }

        /// <summary>
        /// The ToString.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public override string ToString()
        {
            return $"{BaseUrl}, {Headers.Count} Header(s)";
        }
    }
}
