# About 
This the entry point of the solution

## Configuring the projects

1. And the `Startup.cs`'s `ConfigureServices` method will inturn set up all services and resources for the solution (Project 1).
    ```csharp
    public void ConfigureServices(IServiceCollection services)
        {
            // code removed for clarity            
            services.AddHttpClient();
            // config GitHubMockAPI.Services
            Services.ServicesStartup.ConfigureServices(services, Configuration);
        }
    ```