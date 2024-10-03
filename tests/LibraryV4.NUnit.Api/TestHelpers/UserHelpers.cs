using LibraryV4.Contracts.Domain;

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
