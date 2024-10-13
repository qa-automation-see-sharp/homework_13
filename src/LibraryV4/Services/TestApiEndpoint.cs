namespace LibraryV4.Services;

public class TestApiEndpoint
{
    private const string ApiBase = "api";

    public static class Users
    {
        private const string Base = $"{ApiBase}/user";
        public const string Register = $"{Base}/register";

        public static string Login(string nickName, string password)
        {
            return $"{Base}/login?nickname={nickName}&password={password}";
        }
    }

    public static class Books
    {
        private const string Base = $"{ApiBase}/books";

        public static string Create(Guid token)
        {
            return $"{Base}/create?token={token.ToString()}";
        }

        public static string GetBooksByTitle(string title)
        {
            return $"{Base}/by-title/{title}";
        }

        public static string GetBooksByAuthor(string author)
        {
            return $"{Base}/by-author/{author}";
        }

        public static string Delete(string title, string author, string token)
        {
            return $"{Base}/delete?title={title}&author={author}&token={token}";
        }
    }
}