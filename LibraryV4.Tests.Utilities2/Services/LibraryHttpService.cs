using System.Net;
using System.Text;
using Bogus;
using LibraryV4.Contracts.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace LibraryV4.Tests.Services.Services;

public class LibraryHttpService
{
    private WebApplicationFactory<IApiMarker> _factory;
    private readonly HttpClient _httpClient;
    private User? DefaultUser { get; set; }
    private AuthorizationToken? AuthorizationToken { get; set; }

    public LibraryHttpService()
    {
        _factory = new WebApplicationFactory<IApiMarker>();
        _httpClient = _factory.CreateClient();
    }

    public LibraryHttpService Configure(string baseUrl)
    {
        _httpClient.BaseAddress = new Uri(baseUrl);

        return this;
    }

    public async Task<LibraryHttpService> CreateDefaultUser()
    {
        var faker = new Faker();

        DefaultUser = new User()
        {
            FullName = "David Solis",
            NickName = $"soledavi{faker.Random.AlphaNumeric(4)}",
            Password = "126rtgc"
        };

        var url = EndpointsForTest.Users.Register;
        var json = JsonConvert.SerializeObject(DefaultUser);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);
        var jsonString = await response.Content.ReadAsStringAsync();

        Console.WriteLine("Created defaults user: "+jsonString);

        return this;
    }

    public async Task<LibraryHttpService> Authorize()
    {
        var url = EndpointsForTest.Users.Login + $"?nickname={DefaultUser.NickName}&password={DefaultUser.Password}";
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        AuthorizationToken = JsonConvert.DeserializeObject<AuthorizationToken>(content);
        var jsonString = await response.Content.ReadAsStringAsync();

        Console.WriteLine("Authorized with: " + jsonString);

        return this;
    }

    public async Task<HttpResponseMessage> CreateUser(User user)
    {
        var url = EndpointsForTest.Users.Register;
        var json = JsonConvert.SerializeObject(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);
        var jsonString = await response.Content.ReadAsStringAsync();
        WriteLog(response.RequestMessage.Method.ToString(), _httpClient.BaseAddress, url, response.StatusCode, jsonString);

        return response;
    }
    
    public async Task<HttpResponseMessage> LogIn(User user)
    {
        var url = EndpointsForTest.Users.Login + $"?nickname={user.NickName}&password={user.Password}";
        var response = await _httpClient.GetAsync(url);
        var jsonString = await response.Content.ReadAsStringAsync();
        WriteLog(response.RequestMessage.Method.ToString(), _httpClient.BaseAddress, url, response.StatusCode, jsonString);

        return response;
    }
    

    public async Task<HttpResponseMessage> CreateBook(Guid token, Book book)
    {
        var url = EndpointsForTest.Books.Create(token);
        var json = JsonConvert.SerializeObject(book);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);
        var jsonString = await response.Content.ReadAsStringAsync();
        WriteLog(response.RequestMessage.Method.ToString(), _httpClient.BaseAddress, url, response.StatusCode, jsonString);

        return response;
    }
    public async Task<HttpResponseMessage> CreateBook(Book book)
    {
        var url = EndpointsForTest.Books.Create(AuthorizationToken.Token);
        var json = JsonConvert.SerializeObject(book);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);
        var jsonString = await response.Content.ReadAsStringAsync();
        WriteLog(response.RequestMessage.Method.ToString(), _httpClient.BaseAddress, url, response.StatusCode, jsonString);

        return response;
    }

    public async Task<HttpResponseMessage> GetBooksByTitle(string title)
    {
        var url = EndpointsForTest.Books.GetBooksByTitle(title);
        var uri = new Uri(_httpClient.BaseAddress, url);
        var response = await _httpClient.GetAsync(uri);
        var jsonString = await response.Content.ReadAsStringAsync();
        WriteLog(response.RequestMessage.Method.ToString(), _httpClient.BaseAddress, url, response.StatusCode, jsonString);

        return response;
    }
    
    public async Task<HttpResponseMessage> GetBooksByAuthor(string author)
    {
        var url = EndpointsForTest.Books.GetBooksByAuthor(author);
        var uri = new Uri(_httpClient.BaseAddress, url);
        var response = await _httpClient.GetAsync(uri);
        var jsonString = await response.Content.ReadAsStringAsync();
        WriteLog(response.RequestMessage.Method.ToString(), _httpClient.BaseAddress, url, response.StatusCode, jsonString);

        return response;
    }
    
    public async Task<HttpResponseMessage> DeleteBook(Guid token, string title, string author)
    {
        var url = EndpointsForTest.Books.Delete(title, author, token);
        var response = await _httpClient.DeleteAsync(url);
        var jsonString = await response.Content.ReadAsStringAsync();
        WriteLog(response.RequestMessage.Method.ToString(), _httpClient.BaseAddress, url, response.StatusCode, jsonString);

        return response;
    }
    public async Task<HttpResponseMessage> DeleteBook(string title, string author)
    {
        var url = EndpointsForTest.Books.Delete(title, author, AuthorizationToken.Token);
        var response = await _httpClient.DeleteAsync(url);
        var jsonString = await response.Content.ReadAsStringAsync();
        WriteLog(response.RequestMessage.Method.ToString(), _httpClient.BaseAddress, url, response.StatusCode, jsonString);

        return response;
    }

    public void WriteLog(string method, Uri baseAdress, string url, HttpStatusCode statusCode, string content)
    {
        Console.WriteLine(method + " request to: " + baseAdress + url);
        Console.WriteLine("Response status code: " + statusCode);
        Console.WriteLine("Content: " + content);
    }
}