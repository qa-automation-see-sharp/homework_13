using System.Text;
using LibraryV4.Contracts.Domain;
using LibraryV4.NUnit.Api.TestHelpers;
using Newtonsoft.Json;

namespace LibraryV4.NUnit.Api.Services;

public class LibraryHttpService
{
    private readonly HttpClient _httpClient;

    private User? DefaultUser { get; set; }
    private AuthorizationToken? AuthorizationToken { get; set; }

    public LibraryHttpService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        DefaultUser = DataHelper.CreateUser();
    }

    public void Configure(string baseUrl)
    {
        _httpClient.BaseAddress = new Uri(baseUrl);
    }

    public async Task CreateDefaultUser()
    {
        var url = EndpointsForTest.Users.Register;

        var json = JsonConvert.SerializeObject(DefaultUser);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);

        var jsonString = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"POST request to:\n{_httpClient.BaseAddress}{url}");
        Console.WriteLine($"Response Status Code is: {response.StatusCode}");
        Console.WriteLine($"Content: \n{jsonString}");
    }

    public async Task Authorize()
    {
        var url = EndpointsForTest.Users.Login(DefaultUser.NickName, DefaultUser.Password);

        var response = await _httpClient.GetAsync(url);
        var jsonString = await response.Content.ReadAsStringAsync();

        AuthorizationToken = JsonConvert.DeserializeObject<AuthorizationToken>(jsonString);

        Console.WriteLine($"GET request to:\n{_httpClient.BaseAddress}{url}");
        Console.WriteLine($"Response Status Code is: {response.StatusCode}");
        Console.WriteLine($"Content: \n{jsonString}");
    }


    public async Task<HttpResponseMessage> CreateUser(User user)
    {
        var url = EndpointsForTest.Users.Register;

        var json = JsonConvert.SerializeObject(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);
        var jsonString = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"POST request to:\n{_httpClient.BaseAddress}{url}");
        Console.WriteLine($"Response Status Code is: {response.StatusCode}");
        Console.WriteLine($"Content: \n{jsonString}");

        return response;
    }

    public async Task<HttpResponseMessage> LogIn(User user)
    {
        var url = EndpointsForTest.Users.Login(user.NickName, user.Password);

        var response = await _httpClient.GetAsync(url);
        var jsonString = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"GET request to:\n{_httpClient.BaseAddress}{url}");
        Console.WriteLine($"Response Status Code is: {response.StatusCode}");
        Console.WriteLine($"Content: \n{jsonString}");


        return response;
    }


    public async Task<HttpResponseMessage> PostBook(string? token, Book book)
    {
        var url = EndpointsForTest.Books.Create(token);

        var json = JsonConvert.SerializeObject(book);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);
        var jsonString = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"POST request to:\n{_httpClient.BaseAddress}{url}");
        Console.WriteLine($"Response Status Code is: {response.StatusCode}");
        Console.WriteLine($"Content: \n{jsonString}");

        return response;
    }

    public async Task<HttpResponseMessage> PostBook(Book book)
    {
        var url = EndpointsForTest.Books.Create(AuthorizationToken?.Token.ToString());

        var json = JsonConvert.SerializeObject(book);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);
        var jsonString = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"POST request to:\n{_httpClient.BaseAddress}{url}");
        Console.WriteLine($"Response Status Code is: {response.StatusCode}");
        Console.WriteLine($"Content: \n{jsonString}");

        return response;
    }

    public async Task<HttpResponseMessage> GetBooksByTitle(string title)
    {
        var url = EndpointsForTest.Books.GetBooksByTitle(title);

        var response = await _httpClient.GetAsync(url);
        var jsonString = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"GET request to:\n{_httpClient.BaseAddress}{url}");
        Console.WriteLine($"Response Status Code is: {response.StatusCode}");
        Console.WriteLine($"Content: \n{jsonString}");

        return response;
    }

    public async Task<HttpResponseMessage> GetBooksByAuthor(string author)
    {
        var url = EndpointsForTest.Books.GetBooksByAuthor(author);

        var response = await _httpClient.GetAsync(url);
        var jsonString = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"GET request to:\n{_httpClient.BaseAddress}{url}");
        Console.WriteLine($"Response Status Code is: {response.StatusCode}");
        Console.WriteLine($"Content: \n{jsonString}");

        return response;
    }

    public async Task<HttpResponseMessage> DeleteBook(string title, string author)
    {
        var url = EndpointsForTest.Books.Delete(title, author, AuthorizationToken.Token.ToString());

        var response = await _httpClient.DeleteAsync(url);
        var jsonString = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"DELETE request to:\n{_httpClient.BaseAddress}{url}");
        Console.WriteLine($"Response Status Code is: {response.StatusCode}");
        Console.WriteLine($"Content: \n{jsonString}");

        return response;
    }
}