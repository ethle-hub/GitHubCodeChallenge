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
                    webBuilder.UseStartup<Startup>();
                });
    }
}
