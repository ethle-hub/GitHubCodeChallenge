using GitHubMockAPI.Common.Entities;

namespace GitHubMockAPI.Services.Interfaces
{
    using Contracts;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IUsersService" />.
    /// </summary>
    public interface IUsersService
    {
        /// <summary>
        /// The FindUsersAsync.
        /// </summary>
        /// <param name="usernames">The usernames<see cref="List{string}"/>.</param>
        /// <returns>The <see cref="Task{ServiceResult{List{User}}}"/>.</returns>
        Task<ServiceResult<List<User>>> FindUsersAsync(params string[] usernames);
    }
}
