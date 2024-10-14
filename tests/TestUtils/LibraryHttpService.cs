using System.Net;
using System.Text;
using LibraryV4.Contracts.Domain;
using TestUtils;
using Microsoft.AspNetCore.Mvc.Testing;
//using MongoDB.Bson.IO;
using Newtonsoft.Json;

namespace LibraryV4.Services;

public class LibraryHttpService
{
     private readonly HttpClient _httpClient;
    
    private WebApplicationFactory<IApiMarker> _factory = new();

    public readonly Dictionary<User, string> TestUsers = new();

    public LibraryHttpService()
    {
        _httpClient = _factory.CreateClient();
        CreateDefaultUser().Wait();
        LogIn(DefaultUser, true).Wait();
    }

    public User DefaultUser { get; private set; }

    public AuthorizationToken? DefaultUserAuthToken { get; set; }

    public LibraryHttpService Configure(string baseUrl)
    {
        _httpClient.BaseAddress = new Uri(baseUrl);
        return this;
    }

    public async Task CreateDefaultUser()
    {
        DefaultUser = DataHelper.CreateUser();

        var url = TestApiEndpoint.Users.Register;
        var json = JsonConvert.SerializeObject(DefaultUser);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);
        var jsonString = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Created default user:\n{jsonString}");
    }

    public async Task<HttpResponseMessage> LogIn(User user, bool saveTokenAsDefault)
    {
        var url = TestApiEndpoint.Users.Login(user.NickName, user.Password);
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (saveTokenAsDefault) DefaultUserAuthToken = JsonConvert.DeserializeObject<AuthorizationToken>(content);

        Console.WriteLine($"Authorized with user:\n{content}");
        return response;
    }

    public async Task<HttpResponseMessage> CreateUser(User user)
    {
        var url = TestApiEndpoint.Users.Register;
        var json = JsonConvert.SerializeObject(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);
        var jsonString = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == HttpStatusCode.Created) TestUsers.Add(user, string.Empty);


        Console.WriteLine($"Created user:\n{jsonString}");

        return response;
    }

    /*public async Task<HttpResponseMessage> LogIn(User user)
    {
        var url = EndpointsForTest.Users.Login + $"?nickname={user.NickName}&password={user.Password}";
        var response = await _httpClient.GetAsync(url);
        var jsonString = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Logged in with:\n{jsonString}");

        return response;
    }*/

    public async Task<HttpResponseMessage> PostBook(Guid token, Book book)
    {
        var url = TestApiEndpoint.Books.Create(token);
        var json = JsonConvert.SerializeObject(book);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);
        var jsonString = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"POST request to:\n{_httpClient.BaseAddress}{url}");
        Console.WriteLine($"Response Status Code is: {response.StatusCode}");
        Console.WriteLine($"Body: {jsonString}");

        return response;
    }

    public async Task<HttpResponseMessage> GetBooksByTitle(string title)
    {
        var url = TestApiEndpoint.Books.GetBooksByTitle(title);
        var response = await _httpClient.GetAsync(url);
        var jsonString = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"GET request to:\n{_httpClient.BaseAddress}{url}");
        Console.WriteLine($"Response Status Code is: {response.StatusCode}");
        Console.WriteLine($"Content: {jsonString}");

        return response;
    }

    public async Task<HttpResponseMessage> GetBooksByAuthor(string author)
    {
        var url = TestApiEndpoint.Books.GetBooksByAuthor(author);
        var response = await _httpClient.GetAsync(url);
        var jsonString = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"GET request to:\n{_httpClient.BaseAddress}{url}");
        Console.WriteLine($"Response Status Code is: {response.StatusCode}");
        Console.WriteLine($"Content: {jsonString}");

        return response;
    }

    public async Task<HttpResponseMessage> DeleteBook(Guid token, string title, string author)
    {
        var url = TestApiEndpoint.Books.Delete(title, author, token);
        var response = await _httpClient.DeleteAsync(url);
        var jsonString = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Delete request to:\n{_httpClient.BaseAddress}{url}");
        Console.WriteLine($"Response Status Code is: {response.StatusCode}");
        Console.WriteLine($"Content: {jsonString}");

        return response;
    }

}