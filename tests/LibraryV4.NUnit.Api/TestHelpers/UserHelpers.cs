using LibraryV4.Contracts.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryV4.NUnit.Api.TestHelpers
{
    public static class UserHelpers
    {
        public static User CreateUser()
        {
            return new User
            {
                FullName = "Test User",
                NickName = "TestUser" + Guid.NewGuid().ToString(),
                Password = "TestPassword"
            };
        }

        public static User User_ExistNickName()
        {
            return new User
            {
                FullName = "Test User",
                NickName = "TestUser",
                Password = "TestPassword"
            };
        }

        
    }
}
