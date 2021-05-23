namespace GitHubMockAPI.Contracts
{
    /// <summary>
    /// Defines the Users resource, <see cref="User" />.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the Company.
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the Followers.
        /// </summary>
        public int Followers { get; set; }

        /// <summary>
        /// Gets or sets the PublicRepos.
        /// </summary>
        public int PublicRepos { get; set; }

        /// <summary>
        /// Gets the AverageFollowersPerPublicRepos.
        /// </summary>
        public int AverageFollowersPerPublicRepos => (PublicRepos == 0) ? 0 : Followers / PublicRepos;
    }
}
