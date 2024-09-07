using System.Reflection;
using System.Text;
using LibraryV4.Common.Files.For.Tests;
using LibraryV4.Common.Files.For.Tests.TestHelpers;
using LibraryV4.Contracts.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace LibraryV4.xUnit.Tests.Api;

public class LibraryHttpService : IAsyncLifetime
{
    private WebApplicationFactory<IApiMarker> _factory;
    private readonly HttpClient _httpClient;
    public User User;
    public AuthorizationToken Token;

    public LibraryHttpService(){
        _factory = new WebApplicationFactory<IApiMarker>();
        _httpClient = _factory.CreateClient();
        User = DataHelper.UserHelper.CreateRandomUser();
    }

    public async Task InitializeAsync()
    {
        ConsoleHelper.SetUpFromClass(GetType().Name);
        await CreateDefaultUser();
        await Authorize();
    }

    public async Task<User> CreateDefaultUser()
    {
        User = DataHelper.UserHelper.CreateRandomUser();

        var url = EndPointForTests.Users.Register;
        var json = JsonConvert.SerializeObject(User);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);

        ConsoleHelper.Info(HttpMethod.Post, "Create Default User", url, json, response);

        return User;
    }

    public async Task<LibraryHttpService> Authorize()
    {
        var url = EndPointForTests.Users.Login(User.NickName, User.Password);
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

        ConsoleHelper.Info(HttpMethod.Get, MethodBase.GetCurrentMethod().Name, url, jsonString, response);

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

    public async Task DisposeAsync()
    {
        ConsoleHelper.TearDownFromClass(GetType().Name);
    }
}
