# About 
This class library project `GitHubMockAPI.Resources` provide connection to external dependencies such as database or external API link GitHub API used

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
