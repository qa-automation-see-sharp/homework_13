namespace LibraryV4.Tests.Services.Services
{
    internal class EndpointsForTest
    {
        private const string ApiBase = "api";

        public static class Users
        {
            private const string Base = $"{ApiBase}/user";
            public const string Register = $"{Base}/register";
            public const string Login = $"{Base}/login";
        }

        public static class Books
        {
            private const string Base = $"{ApiBase}/books";

            public static string Create(Guid token) => $"{Base}/create/?token={token}";
            public static string GetBooksByTitle(string title) => $"{Base}/by-title/{title}";
            public static string GetBooksByAuthor(string author) => $"{Base}/by-author/{author}";
            public static string Delete(string title, string author, Guid token) => $"{Base}/delete/?title={title}&author={author}&token={token}";
        }
    }
}
