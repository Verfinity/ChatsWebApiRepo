using ChatsWebApi.Components.Types.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatsWebApiTests.UsersControllerTest
{
    public static class UsersHelper
    {
        public static List<User> GetTestUsers()
        {
            return new List<User>
            {
                new User
                {
                    Id = 1,
                    FirstName = "AA",
                    LastName = "AA",
                    NickName = "AA",
                    Password = "12345678"
                },
                new User
                {
                    Id = 2,
                    FirstName = "BB",
                    LastName = "BB",
                    NickName = "BB",
                    Password = "12345678"
                }
            };
        }
    }
}
