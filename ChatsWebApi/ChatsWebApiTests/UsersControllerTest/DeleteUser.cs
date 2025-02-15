using ChatsWebApi.Components.Controllers;
using ChatsWebApi.Components.Repositories.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ChatsWebApiTests.UsersControllerTest
{
    public class DeleteUser
    {
        [Fact]
        public async Task Should_Returns_Ok_Without_Data()
        {
            var mock = new Mock<IUsersRepository>();
            mock.Setup(repo => repo.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);
            var usersController = new UsersController(mock.Object);

            var answer = await usersController.DeleteUserAsync(It.IsAny<int>());

            Assert.IsType<OkResult>(answer);
        }

        [Fact]
        public async Task Should_Returns_Bad_Request_Without_Data()
        {
            var mock = new Mock<IUsersRepository>();
            mock.Setup(repo => repo.DeleteAsync(It.IsAny<int>())).ReturnsAsync(false);
            var usersController = new UsersController(mock.Object);

            var answer = await usersController.DeleteUserAsync(It.IsAny<int>());

            Assert.IsType<BadRequestResult>(answer);
        }
    }
}
