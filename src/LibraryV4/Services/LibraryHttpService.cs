using System;
using System;
using System;
using System.Net;
using System.Collections.Generic;
using LibraryV4;
using LibraryV4.Endpoints;
using LibraryV4.Contracts.Domain;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.OpenApi.Validations;
//using MongoDB.Bson.IO;
using Newtonsoft.Json;

namespace LibraryV4.Services
{
    public class LibraryHttpService
    {
        private WebApplicationFactory<IApiMarker> _factory;
        private readonly HttpClient _client;
        private User? _testUser { get; set; }
        private Book? _testBook { get; set; }

        private List<List<Book>> _library { get; set; }
        public List<List<Book>> Library => _library;
        private AuthorizationToken? _authorizationToken { get; set; }

        // Constructor for LibraryHttpService
        public LibraryHttpService()
        {
            _factory = new WebApplicationFactory<IApiMarker>();
            _client = _factory.CreateClient();
        }

        //Configure method for LibraryHttpService
        public LibraryHttpService Configure(string baseUrl)
        {
            _client.BaseAddress = new Uri(baseUrl);
            return this;
        }

        //Creating a test user
        public async Task<LibraryHttpService> CreateTestUser()
        {
            _testUser = new User
            {
                FullName = "Test User",
                NickName = "TestUser",
                Password = "TestPassword"
            };

            var url = EndpointsForTests.Users.Register;
            var json = JsonConvert.SerializeObject(_testUser);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, content);
            var jsonResult = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Created test user: {jsonResult}");

            return this;
        }

        //Authorizing the test user
        public async Task<LibraryHttpService> LoginTestUser()
        {
            var url = EndpointsForTests.Users.Login + $"?nickName={_testUser.NickName}&password={_testUser.Password}";
            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            _authorizationToken = JsonConvert.DeserializeObject<AuthorizationToken>(content);
            var jsonResult = await response.Content.ReadAsStringAsync();

            return this;
        }

        public async Task<LibraryHttpService> CreateTestBook()
        {
            _testBook = new Book
            {
                Title = "Test Book",
                Author = "Test Author",
                YearOfRelease = 2021
            };

            var url = EndpointsForTests.Books.Create(_authorizationToken.Token);
            var json = JsonConvert.SerializeObject(_testBook);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, content);
            var jsonResult = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Created test Book: {jsonResult}");

            return this;
        }

        //Creating a user
        public async Task<HttpResponseMessage> CreateUser(User user)
        {
            var url = EndpointsForTests.Users.Register;
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, content);
            var jsonResult = await response.Content.ReadAsStringAsync();
            WriteLog(response.RequestMessage.Method.ToString(), _client.BaseAddress, url, response.StatusCode, jsonResult);

            return response;
        }

        //Login user
        public async Task<HttpResponseMessage> LogIn(User user)
        {
            var url = EndpointsForTests.Users.Login + $"?nickName={user.NickName}&password={user.Password}";
            var response = await _client.GetAsync(url);
            var jsonResult = await response.Content.ReadAsStringAsync();
            WriteLog(response.RequestMessage.Method.ToString(), _client.BaseAddress, url, response.StatusCode, jsonResult);

            return response;
        }

        //Creating a book
        public async Task<HttpResponseMessage> CreateBook(Guid token, Book book)
        {
            var url = EndpointsForTests.Books.Create(token);
            var json = JsonConvert.SerializeObject(book);
            var contet = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, contet);
            var jsonResult = await response.Content.ReadAsStringAsync();
            WriteLog(response.RequestMessage.Method.ToString(), _client.BaseAddress, url, response.StatusCode, jsonResult);

            return response;
        }

        //Create book
        public async Task<HttpResponseMessage> CreateBook(Book book)
        {
            var url = EndpointsForTests.Books.Create(_authorizationToken.Token);
            var json = JsonConvert.SerializeObject(book);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, content);
            var jsonResult = await response.Content.ReadAsStringAsync();
            WriteLog(response.RequestMessage.Method.ToString(), _client.BaseAddress, url, response.StatusCode, jsonResult);

            return response;
        }



        public async Task<HttpResponseMessage> GetBooksByTitle(string title)
        {
            var url = EndpointsForTests.Books.GetBooksByTitle(title);
            var uri = new Uri(_client.BaseAddress, url);
            var response = await _client.GetAsync(uri);
            var jsonResult = await response.Content.ReadAsStringAsync();
            WriteLog(response.RequestMessage.Method.ToString(), _client.BaseAddress, url, response.StatusCode, jsonResult);

            return response;
        }

        public async Task<HttpResponseMessage> GetBooksByAuthor(string author)
        {
            var url = EndpointsForTests.Books.GetBooksByAuthor(author);
            var uri = new Uri(_client.BaseAddress, url);
            var response = await _client.GetAsync(uri);
            var jsonResult = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Request: {uri}, Status: {response.StatusCode}, Response: {jsonResult}");

            WriteLog(response.RequestMessage.Method.ToString(), _client.BaseAddress, url, response.StatusCode, jsonResult);

            return response;
        }
       
        public async Task<HttpResponseMessage> DeleteBook(Guid token, string title, string author)
        {
            var url = EndpointsForTests.Books.Delete(title, author, token);
            var response = await _client.DeleteAsync(url);
            var jsonResult = await response.Content.ReadAsStringAsync();
            WriteLog(response.RequestMessage.Method.ToString(), _client.BaseAddress, url, response.StatusCode, jsonResult);

            return response;
        }
        public async Task<HttpResponseMessage> DeleteBook(string title, string author)
        {
            var url = EndpointsForTests.Books.Delete(title, author, _authorizationToken.Token);
            var uri = new Uri(_client.BaseAddress, url);
            var response = await _client.DeleteAsync(uri);
            var jsonResult = await response.Content.ReadAsStringAsync();
            WriteLog(response.RequestMessage.Method.ToString(), _client.BaseAddress, url, response.StatusCode, jsonResult);

            return response;
        }
        //Writing logs
        public void WriteLog(string method, Uri baseAddress, string url, HttpStatusCode statusCode, string content)
        {
            Console.WriteLine($"Method: {method}");
            Console.WriteLine($"BaseAddress: {baseAddress}");
            Console.WriteLine($"StatusCode: {statusCode}");
            Console.WriteLine($"Content: {content}");
        }

    }
}
