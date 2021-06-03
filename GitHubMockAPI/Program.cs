namespace GitHubMockAPI
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Defines the <see cref="Program" />.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The Main.
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/>.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// The CreateHostBuilder.
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/>.</param>
        /// <returns>The <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config.AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);

                    // Register the user secrets configuration source
                    if (env.IsDevelopment())
                        config.AddUserSecrets<Program>();

                    config.AddEnvironmentVariables(); // overwrites previous values
                })
                //.ConfigureLogging((hostingContext, logBuilder) =>
                //{
                //    logBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                //    logBuilder.ClearProviders();   // removes all providers from LoggerFactory
                //    logBuilder.AddConsole();
                //    logBuilder.AddDebug();
                //})
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // use Azure App Configuration
                    // so the configuration provider for App Configuration has been registered with the .NET Core Configuration API.
                    webBuilder.ConfigureAppConfiguration(config =>
                    {
                        var settings = config.Build();

                        // Read the secret via the Configuration API https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-5.0&tabs=windows
                        // TODO: Map secrets to a POCO
                        var connection = settings.GetConnectionString("AppConfig"); 
                        config.AddAzureAppConfiguration(options =>
                            options.Connect(connection).UseFeatureFlags());
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
