using GitHubMockAPI.Resources.GitHub;
using Microsoft.Extensions.Options;

namespace GitHubMockAPI.Resources
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// To setup the services and dependencies used in `GitHubMockAPI.Resources` project.
    /// To be called in `ServicesStartup.cs` of `GitHubMockAPI.Services` project. 
    /// </summary>
    public static class Startup
    {
        /// <summary>
        /// The ConfigureServices.
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/>.</param>
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // make IOptions<GitHubApiOptions>
            services.Configure<GitHubApiOptions>(options => configuration.GetSection("Resources:GitHubApi:Options").Bind(options));
            
            // and then make a single instance of GitHubApiOptions
            // so that we can inject it anywhere later 
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<GitHubApiOptions>>().Value);
            
            // define this client to call GitHub API
            services.AddSingleton<IGitHubApi, GitHubApiClient>();
        }
    }
}
