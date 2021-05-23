namespace GitHubMockAPI.Tests
{
    using Common.Entities;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Resources.GitHub;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Xunit;

    /// <summary>
    /// Defines the <see cref="GitHubClientTests" />.
    /// </summary>
    public class GitHubClientTests
    {
        /// <summary>
        /// Defines the HttpClient.
        /// </summary>
        private static readonly HttpClient HttpClient = new();

        /// <summary>
        /// The Should_Return_Unsuccessful_ServiceResult.
        /// </summary>
        /// <param name="serviceBaseUrl">The serviceBaseUrl<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("https://")]
        [InlineData("https://api.github.com")]
        public async Task Should_Return_Unsuccessful_ServiceResult(string serviceBaseUrl)
        {
            // arrange
            var loggerMock = new Mock<ILogger<GitHubApiClient>>();
            var httpClientConfiguration = new GitHubApiOptions(serviceBaseUrl);
            var client = new GitHubApiClient(loggerMock.Object, HttpClient, httpClientConfiguration);

            // act
            var result = await client.GetUser(It.IsAny<string>());

            // assert
            Assert.IsType<ServiceResult<GitHubApiUser>>(result);
            Assert.False(result.Outcome.IsSuccess);
            Assert.False(string.IsNullOrWhiteSpace(result.Outcome.ErrorMessage));
            Assert.True(result.Data == null);
        }

        /// <summary>
        /// The Should_Return_Successful_ServiceResult.
        /// </summary>
        /// <param name="serviceBaseUrl">The serviceBaseUrl<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [Theory]
        [InlineData("https://api.github.com")]
        public async Task Should_Return_Successful_ServiceResult(string serviceBaseUrl)
        {
            // arrange
            var loggerMock = new Mock<ILogger<GitHubApiClient>>();
            var httpClientConfiguration = new GitHubApiOptions(serviceBaseUrl, new Dictionary<string, string>(){
                {"Code-Challenge-Exercise-App", "1"}
            });

            var client = new GitHubApiClient(loggerMock.Object, HttpClient, httpClientConfiguration);

            // act
            var result = await client.GetUser("microsoft");

            // assert
            Assert.IsType<ServiceResult<GitHubApiUser>>(result);
            Assert.True(result.Outcome.IsSuccess);
            Assert.False(result.Data == null);
            Assert.IsType<GitHubApiUser>(result.Data);
            Assert.Equal("microsoft", result.Data.Login.ToLower());
        }
    }
}
