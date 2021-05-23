namespace GitHubMockAPI.Tests
{
    using Contracts;
    using Controllers;
    using Services;
    using Services.Interfaces;
    using Microsoft.Extensions.Logging;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    /// <summary>
    /// Defines the <see cref="UsersControllerTests" />.
    /// </summary>
    public class UsersControllerTests
    {
        
        /// <summary>
        /// The Should_Return_Type_ApiResponse.
        /// </summary>
        /// <param name="usernames">The usernames<see cref="string[]"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [Theory]
        [InlineData(null)]
        [InlineData(null, "microsoft", "dummy")]
        public async Task Should_Return_Type_ApiResponse(params string[] usernames)
        {
            // arrange
            var loggerMock = new Mock<ILogger<UsersController>>();
            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(dummy => dummy.FindUsersAsync(It.IsAny<string[]>()))
                .ReturnsAsync(new List<User>());

            var controller = new UsersController(loggerMock.Object, usersServiceMock.Object);

            // act
            var result = await controller.RetrieveUsers(usernames?.ToList().ConvertToApiRequest());

            // assert
            Assert.IsType<ApiResponse<List<User>>>(result);

            if (usernames == null)
            {
                Assert.False(result.IsSuccess);
                Assert.False(string.IsNullOrWhiteSpace(result.ErrorMessage));
            }
            else
            {
                Assert.True(result.IsSuccess);
                Assert.True(string.IsNullOrWhiteSpace(result.ErrorMessage));

            }
        }

        [Fact]
        public async Task Should_Handle_Divide_By_Zero_Exception()
        {
            // arrange
            var loggerMock = new Mock<ILogger<UsersController>>();
            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(dummy => dummy.FindUsersAsync(It.IsAny<string[]>()))
                .ReturnsAsync(new List<User>
                {
                    new(){Name = "B", Login = "Login B", Company = "", Followers = 3, PublicRepos = 0}
                });

            var controller = new UsersController(loggerMock.Object, usersServiceMock.Object);

            // act
            var result = await controller.RetrieveUsers(new List<string>{"Login B"}.ConvertToApiRequest());

            // assert
            Assert.IsType<ApiResponse<List<User>>>(result);
            Assert.True(result.IsSuccess);
            Assert.True(result.Data.Exists(ur => ur.Login.Equals("Login B") && ur.AverageFollowersPerPublicRepos == 0));
        }
    }
}
