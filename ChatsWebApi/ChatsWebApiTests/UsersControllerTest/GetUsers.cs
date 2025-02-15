using ChatsWebApi.Components.Controllers;
using ChatsWebApi.Components.Repositories.Users;
using ChatsWebApi.Components.Types.Database;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ChatsWebApiTests.UsersControllerTest
{
    public class GetUsers
    {
        [Fact]
        public async Task Should_Returns_No_Content()
        {
            var mock = new Mock<IUsersRepository>();
            mock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<User>());
            var usersController = new UsersController(mock.Object);

            var answer = await usersController.GetUsersAsync();

            Assert.IsType<NoContentResult>(answer.Result);
        }

        [Fact]
        public async Task Should_Returns_List_Of_Objects()
        {
            var testUsers = UsersHelper.GetTestUsers();
            var mock = new Mock<IUsersRepository>();
            mock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(testUsers);
            var usersController = new UsersController(mock.Object);

            var answer = await usersController.GetUsersAsync();

            var actionResult = Assert.IsType<ActionResult<List<User>>>(answer);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var result = Assert.IsType<List<User>>(okResult.Value);
            Assert.Equal(testUsers, result);
        }
    }
}
