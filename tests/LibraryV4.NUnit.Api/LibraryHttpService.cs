using System.Text;
using DocsForTests;
using DocsForTests.TestHelpers;
using LibraryV4.Contracts.Domain;
using Newtonsoft.Json;

namespace LibraryV4.NUnit.Api
{
    public class LibraryHttpService
    {
        private readonly HttpClient _httpClient;
        private AuthorizationToken Token { get; set; }
        private User? DefaultUser { get; set; }

        public LibraryHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User> CreateDefaultUser()
        {
            DefaultUser = DataHelper.UserHelper.CreateRandomUser();

            var url = EndPointForTests.Users.Register;
            var json = JsonConvert.SerializeObject(DefaultUser);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            ConsoleHelper.Info(HttpMethod.Post, "Create Default User", url, json, response);

            return DefaultUser;
        }

        public async Task<LibraryHttpService> Authorize()
        {
            var url = EndPointForTests.Users.Login(DefaultUser.NickName, DefaultUser.Password);
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            Token = JsonConvert.DeserializeObject<AuthorizationToken>(content);

            ConsoleHelper.Info(HttpMethod.Get, "Authorize", url, content, response);

            return this;
        }


        public async Task<HttpResponseMessage> CreateUser(User user)
        {
            var url = EndPointForTests.Users.Register;
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            var jsonString = await response.Content.ReadAsStringAsync();

            ConsoleHelper.Info(HttpMethod.Post, "Create user", url, jsonString, response);

            return response;
        }

        public async Task<HttpResponseMessage> LogIn(User user)
        {
            var url = EndPointForTests.Users.Login(user.NickName, user.Password);
            var response = await _httpClient.GetAsync(url);
            var jsonString = await response.Content.ReadAsStringAsync();

            ConsoleHelper.Info(HttpMethod.Get, "Login", url, jsonString, response);

            return response;
        }

        public async Task<HttpResponseMessage> LogIn(string nickName, string password)
        {
            var url = EndPointForTests.Users.Login(nickName, password);
            var response = await _httpClient.GetAsync(url);
            var jsonString = await response.Content.ReadAsStringAsync();

            ConsoleHelper.Info(HttpMethod.Get, "Login", url, jsonString, response);

            return response;
        }


        public async Task<HttpResponseMessage> PostBook(Book book)
        {
            var url = EndPointForTests.Books.Create(Token.Token.ToString());
            var json = JsonConvert.SerializeObject(book);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            var jsonString = await response.Content.ReadAsStringAsync();

            ConsoleHelper.Info(HttpMethod.Post, "Create book", url, jsonString, response);

            return response;
        }

        public async Task<HttpResponseMessage> GetBooksByTitle(string title)
        {
            var url = EndPointForTests.Books.GetBooksByTitle(title);
            var response = await _httpClient.GetAsync(url);
            var jsonString = await response.Content.ReadAsStringAsync();

            ConsoleHelper.Info(HttpMethod.Get, "Get book by title", url, jsonString, response);

            return response;
        }

        public async Task<HttpResponseMessage> GetBooksByAuthor(string author)
        {
            var url = EndPointForTests.Books.GetBooksByAuthor(author);
            var response = await _httpClient.GetAsync(url);
            var jsonString = await response.Content.ReadAsStringAsync();

            ConsoleHelper.Info(HttpMethod.Get, "Get book by author", url, jsonString, response);

            return response;
        }

        public async Task<HttpResponseMessage> DeleteBook(string title, string author)
        {
            var url = EndPointForTests.Books.Delete(title, author, Token.Token.ToString());
            var response = await _httpClient.DeleteAsync(url);
            var jsonString = await response.Content.ReadAsStringAsync();

            ConsoleHelper.Info(HttpMethod.Delete, "Delete book", url, jsonString, response);

            return response;
        }

        public async Task<HttpResponseMessage> DeleteBook(string title, string author, string token)
        {
            var url = EndPointForTests.Books.Delete(title, author, token);
            var response = await _httpClient.DeleteAsync(url);
            var jsonString = await response.Content.ReadAsStringAsync();

            ConsoleHelper.Info(HttpMethod.Delete, "Delete book", url, jsonString, response);

            return response;
        }
    }
}