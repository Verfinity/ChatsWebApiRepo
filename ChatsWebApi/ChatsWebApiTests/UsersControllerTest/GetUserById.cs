using ChatsWebApi.Components.Controllers;
using ChatsWebApi.Components.Repositories.Users;
using ChatsWebApi.Components.Types.Database;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ChatsWebApiTests.UsersControllerTest
{
    public class GetUserById
    {
        [Fact]
        public async Task Should_Returns_No_Content()
        {
            var mock = new Mock<IUsersRepository>();
            mock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((User?)null);
            var usersController = new UsersController(mock.Object);

            var answer = await usersController.GetUserByIdAsync(It.IsAny<int>());

            var actionResult = Assert.IsType<ActionResult<User?>>(answer);
            Assert.IsType<NoContentResult>(actionResult.Result);
        }

        [Fact]
        public async Task Should_Returns_Single_Object()
        {
            var testUsers = UsersHelper.GetTestUsers();
            var mock = new Mock<IUsersRepository>();
            mock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(testUsers[0]);
            var usersController = new UsersController(mock.Object);

            var answer = await usersController.GetUserByIdAsync(It.IsAny<int>());

            var actionResult = Assert.IsType<ActionResult<User>>(answer);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var result = Assert.IsType<User>(okResult.Value);
            Assert.Equal(testUsers[0], result);
        }
    }
}
