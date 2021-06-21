#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["GitHubMockAPI/GitHubMockAPI.csproj", "GitHubMockAPI/"]
COPY ["GitHubMockAPI.Service/GitHubMockAPI.Services.csproj", "GitHubMockAPI.Service/"]
COPY ["GitHubMockAPI.Contracts/GitHubMockAPI.Contracts.csproj", "GitHubMockAPI.Contracts/"]
COPY ["GitHubMockAPI.Common/GitHubMockAPI.Common.csproj", "GitHubMockAPI.Common/"]
COPY ["GitHubMockAPI.Resources/GitHubMockAPI.Resources.csproj", "GitHubMockAPI.Resources/"]
RUN dotnet restore "GitHubMockAPI/GitHubMockAPI.csproj"
COPY . .
WORKDIR "/src/GitHubMockAPI"
RUN dotnet build "GitHubMockAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GitHubMockAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GitHubMockAPI.dll"]