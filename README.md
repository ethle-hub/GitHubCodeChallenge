# About
This  coding excersice is to mimic GitHub API writen in .NET Core 5 Web API. 

## Running the sample
1. Open `GitHubCodeChallenge.sln` in Visual Studio 19
2. Build Solution (F6)
3. Start Debugging (F5)
4. Make Http request with `curl` command as below

```console
curl -X POST "https://localhost:{your-port}/api/Users" -H  "accept: application/json" -H  "Content-Type: application/json" -d "{\"data\":[\"your-name\",\"microsoft\"]}"
```


## Code Structure

There are 6 projects grouped into 5 logical folders in the solution

```
..
+-- APIs
|   +-- GitHubMockAPI   (1)
+-- Contracts
|   +-- GitHubMockAPI.Common    (2)
|   +-- GitHubMockAPI.Contracts (3)
+-- Resources
|   +-- GitHubMockAPI.Resources (4)
+-- Services
|   +-- GitHubMockAPI.Services  (5)
+-- Tests
|   +-- GitHubMockAPI.Tests (6)

```
1. This is the entry point which mimic the GitHub API to lookup user rofile by usernames.
2. This project includes shared entities and models used in the solutions.
3. This project defines API's data contracts for consumers such as `ApiRequest.cs` and `ApiResponse.cs`
4. This project setups external resources to be used in the solution. Note that there is only dependency which is the `GitGubApiClient.cs`
5. This project prepresent the business layer for the `GitHubMockAPI`
6. This projects contains all unit tests and integration tests


### Configuring the projects

1. Ensure your settings in `appsettings.Development.json` as follow

```json
 {
  // removed for clarity
  
  // re: GitHubMockAPI.Resources
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
  

  // re: GitHubMockAPI.Services
  "Services": {
    "UsersService": {
      "Options": {
        "CacheExpirationInMinute": "2"
      }
    }
  }
}  
 ```


2. See *README.md* in the project `GitHubMockAPI.Resources` (Project 4).

3. See *README.md* in the project `GitHubMockAPI.Services` (Project 5).

4. See *README.md* in the project `GitHubMockAPI` (Project 1).