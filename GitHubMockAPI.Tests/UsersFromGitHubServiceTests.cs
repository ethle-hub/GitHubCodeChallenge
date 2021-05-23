using Microsoft.Extensions.DependencyInjection;

namespace GitHubMockAPI.Tests
{
    using Common.Entities;
    using Contracts;
    using Resources.GitHub;
    using Services.Implementations;
    using Services.Interfaces;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    /// <summary>
    /// Defines the <see cref="UsersFromGitHubServiceTests" />.
    /// </summary>
    public class UsersFromGitHubServiceTests
    {
        /// <summary>
        /// Gets the TestUsersData.
        /// </summary>
        private static IEnumerable<User> TestUsersData => new List<User>
        {
            new(){Name = "C", Login = "Login C", Company = "", Followers = 6, PublicRepos = 2},
            new(){Name = "B", Login = "Login B", Company = "", Followers = 3, PublicRepos = 0},
            new(){Name = "A", Login = "Login A", Company = "", Followers = 1, PublicRepos = 3}
        };

        /// <summary>
        /// The CreateGitHubApiMock.
        /// </summary>
        /// <returns>The <see cref="Mock{IGitHubApi}"/>.</returns>
        private Mock<IGitHubApi> CreateGitHubApiMock()
        {
            var gitHubApiMock = new Mock<IGitHubApi>();
            gitHubApiMock.Setup(api => api.GetUser("Login A"))
                .ReturnsAsync(new GitHubApiUser { Name = "A", Login = "Login A", Company = "", Followers = 1, PublicRepos = 3 });
            gitHubApiMock.Setup(api => api.GetUser("Login B"))
                .ReturnsAsync(new GitHubApiUser { Name = "B", Login = "Login B", Company = "", Followers = 3, PublicRepos = 0 });
            gitHubApiMock.Setup(api => api.GetUser("Login C"))
                .ReturnsAsync(new GitHubApiUser { Name = "C", Login = "Login C", Company = "", Followers = 6, PublicRepos = 2 });
            gitHubApiMock.Setup(api => api.GetUser("Login D"))
                .ReturnsAsync(ServiceOutcome.Error(ErrorType.BusinessError, "Not found"));
            return gitHubApiMock;
        }

        /// <summary>
        /// The CreateMemoryCacheMockWithCacheItem.
        /// </summary>
        /// <param name="expectedValue">The expectedValue<see cref="object"/>.</param>
        /// <returns>The <see cref="IMemoryCache"/>.</returns>
        private static IMemoryCache CreateMemoryCacheMockWithCacheItem(object expectedValue)
        {
            var mockMemoryCache = new Mock<IMemoryCache>();
            mockMemoryCache
                .Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue))
                .Returns(false);

            mockMemoryCache
                .Setup(x => x.Set(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<TimeSpan>()))
                .Returns(new object());


            return mockMemoryCache.Object;
        }

        private IMemoryCache CreateMemoryCacheInstance()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();

            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            return memoryCache;
        }

        /// <summary>
        /// The Should_Return_Type_ServiceResult.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Should_Return_Type_ServiceResult()
        {
            // arrange
            var loggerMock = new Mock<ILogger<UsersFromGitHubService>>();
            var gitHubApiMock = new Mock<IGitHubApi>();
            var cacheInstance = CreateMemoryCacheInstance();// new Mock<IMemoryCache>();

            var usersService = new UsersFromGitHubService(loggerMock.Object, gitHubApiMock.Object, cacheInstance, new UsersFromGitHubServiceOptions());

            // act
            var result = await usersService.FindUsersAsync(It.IsAny<string[]>());

            // assert
            Assert.IsType<ServiceResult<List<User>>>(result);
        }

        /// <summary>
        /// The Should_Sort_Users_Alphabetically_By_Name.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Should_Sort_Users_Alphabetically_By_Name()
        {
            // arrange
            var loggerMock = new Mock<ILogger<UsersFromGitHubService>>();
            var gitHubApiMock = CreateGitHubApiMock();
            var cacheMock = new Mock<IMemoryCache>();

            var usersService = new UsersFromGitHubService(loggerMock.Object, gitHubApiMock.Object, cacheMock.Object, new UsersFromGitHubServiceOptions());

            // act
            var result = await usersService.FindUsersAsync("Login B", "Login C", "Login A");

            var actualOrder = result.Data;
            var expectedOrder = TestUsersData.OrderBy(r => r.Name).ToList();

            // assert
            Assert.IsType<ServiceResult<List<User>>>(result);
            Assert.True(result.Outcome.IsSuccess);
            Assert.False(result.Data == null);
            Assert.True(result.Outcome.ErrorType == ErrorType.None);

            Assert.Equal(expectedOrder.Count, actualOrder.Count);
            Assert.Equal(expectedOrder.First().Name, actualOrder.First().Name);
            Assert.Equal(expectedOrder.Last().Name, actualOrder.Last().Name);
        }

        /// <summary>
        /// The Should_Return_Users_If_Available.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [Fact]
        public async Task Should_Return_Users_If_Available()
        {
            var userCAD = new[] { "Login C", "Login A", "Login D" };

            // arrange #1
            var usersService = new Mock<IUsersService>();
            usersService.Setup(dummy => dummy.FindUsersAsync(It.IsAny<string[]>()))
                .ReturnsAsync(TestUsersData.Where(ur => userCAD.Contains(ur.Login)).ToList());

            // act #1
            var result = await usersService.Object.FindUsersAsync(userCAD);

            // assert #1
            Assert.IsType<ServiceResult<List<User>>>(result);
            Assert.True(result.Outcome.IsSuccess);
            Assert.False(result.Data == null);
            Assert.True(result.Outcome.ErrorType == ErrorType.None);

            Assert.Equal(2, result.Data.Count);
            Assert.False(result.Data.Exists(user => user.Login.Equals("Login D")));


            // arrange #2
            var userCADB = userCAD.Concat(new[] { "Login B" }).ToArray();
            usersService.Setup(dummy => dummy.FindUsersAsync(It.IsAny<string[]>()))
                .ReturnsAsync(TestUsersData.Where(ur => userCADB.Contains(ur.Login)).ToList());

            // act #2
            result = await usersService.Object.FindUsersAsync(userCADB);

            // assert #2
            Assert.IsType<ServiceResult<List<User>>>(result);
            Assert.True(result.Outcome.IsSuccess);
            Assert.False(result.Data == null);
            Assert.True(result.Outcome.ErrorType == ErrorType.None);

            Assert.Equal(3, result.Data.Count);
            Assert.True(result.Data.Exists(user => user.Login.Equals("Login B")));
        }

        
        [Fact]
        public async Task Should_Get_User_From_Cache_When_Available()
        {
            // arrange
            var loggerMock = new Mock<ILogger<UsersFromGitHubService>>();
            var gitHubApiMock = CreateGitHubApiMock();
            var cacheInstance = CreateMemoryCacheInstance();

            cacheInstance.Set("Login B", TestUsersData.First(ur => ur.Login.Equals("Login B")), TimeSpan.FromSeconds(2));

            var usersService = new UsersFromGitHubService(loggerMock.Object, gitHubApiMock.Object, cacheInstance, new UsersFromGitHubServiceOptions(){CacheExpirationInMinute = 1});
            
            // act
            var result = await usersService.FindUsersAsync("Login B", "Login C", "Login D");

            // assert
            Assert.IsType<ServiceResult<List<User>>>(result);
            Assert.True(result.Data.Exists(ur => ur.Login.Equals("Login B")));
        }

    }
}
