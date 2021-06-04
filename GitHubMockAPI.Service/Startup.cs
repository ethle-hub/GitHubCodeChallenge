namespace GitHubMockAPI.Services
{
    using Implementations;
    using Interfaces;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// To setup the services and dependencies used in `GitHubMockAPI.Services` project.
    /// To be called in `Startup.cs` of `GitHubMockAPI.GitHubMockAPI` project.
    /// </summary>
    public static class Startup
    {
        /// <summary>
        /// The ConfigureServices.
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/>.</param>
        //public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        public static IServiceCollection AddGitHubMockAPIServices(this IServiceCollection services, IConfiguration configuration)
        {
            // init GitHubMockAPI.Resources
            //services.AddGitHubMockAPIResources(configuration); // see GitHubMockAPI.Startup.cs

            // use caching
            services.AddMemoryCache();

            // make IUsersService implementation
            services.Configure<UsersFromGitHubServiceOptions>(options => configuration.GetSection("Services:UsersService:Options").Bind(options));
            // and then make a single instance of UsersFromGitHubServiceOptions
            // so that we can inject it anywhere later
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<UsersFromGitHubServiceOptions>>().Value);

            services.AddScoped<IUsersService, UsersFromGitHubService>();
            return services;
        }
    }
}
