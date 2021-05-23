# About 
This class library project `GitHubMockAPI.Services` provides resource lookup with business logic for the `GitHubMockAPI` project

## Configuring the projects

1. Ensure the related settings section in `appsettings.Development.json` as follow

```json
 {
  // removed for clarity
  
  "Resources": {
    "GitHubApi": {
      "Options": {
        "BaseUrl": "https://api.github.com",
        "Headers": 
        {
          "Code-Challenge-Exercise-App": "1"
        }
      }
    }
  },
  
}  
 ```

 2. To initialise, call the `GitHubMockAPI.Resources.Startup.cs` on the method name `ConfigureServices`


2. The `ResourcesStartup.cs`'s `ConfigureServices` method will need `"Resources"`configuration setting (Project 4).

3. The `ServicesStartup.cs`'s `ConfigureServices` method will need `"Services"`configuration setting (Project 5).

4. And the `Startup.cs`'s `ConfigureServices` method will inturn set up all services and resources for the solution (Project 1).
    ```csharp
    public void ConfigureServices(IServiceCollection services)
        {
            // code removed for clarity            
            services.AddHttpClient();
            // config GitHubMockAPI.Services
            Services.ServicesStartup.ConfigureServices(services, Configuration);
        }
    ```
