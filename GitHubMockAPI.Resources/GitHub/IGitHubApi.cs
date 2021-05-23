using GitHubMockAPI.Common.Entities;

namespace GitHubMockAPI.Resources.GitHub
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IGitHubApi" />.
    /// </summary>
    public interface IGitHubApi
    {
        /// <summary>
        /// The GetUser.
        /// </summary>
        /// <param name="username">The username<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{ServiceResult{GitHubApiUser}}"/>.</returns>
        Task<ServiceResult<GitHubApiUser>> GetUser(string username);
    }
}
