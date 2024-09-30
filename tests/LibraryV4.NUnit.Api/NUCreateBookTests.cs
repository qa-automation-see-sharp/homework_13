using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Security.Cryptography.X509Certificates;
using LibraryV4.Contracts.Domain;
using LibraryV4.Services;
using LibraryV4.NUnit.Api.TestHelpers;


namespace LibraryV4.NUnit.Tests.Api.Tests
{
    public class CreateBookTests
    {
        private Guid _token = Guid.Empty;
        private Book _testBook;
        private LibraryHttpService _libraryHttpService;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            _libraryHttpService = new LibraryHttpService();
            await _libraryHttpService.CreateTestUser();
            await _libraryHttpService.LoginTestUser();
            await _libraryHttpService.CreateTestBook();
        }

        //Create a book with no Authorisaton token
        [Test]
        public async Task CreateBook_IfNoAuthorizationToken_Return_Unauthorized()
        {
            //Arrange
            _testBook = BookHelpers.CreateBook();
            
            //Act
            var response = await _libraryHttpService.CreateBook(_token, _testBook);
            var jsonResult = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }

        //Create a book with new title and author
        [Test]
        public async Task CreateBook_IfNewTitleAndAuthor_Return_Created()
        {
            _testBook = BookHelpers.CreateBook();
            
            var response = await _libraryHttpService.CreateBook(_testBook);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var newBook = JsonConvert.DeserializeObject<Book>(jsonResult);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(jsonResult, Is.Not.Null);
                Assert.That(newBook.Title, Is.EqualTo(_testBook.Title));
                Assert.That(newBook.Author, Is.EqualTo(_testBook.Author));
                Assert.That(newBook.YearOfRelease, Is.EqualTo(_testBook.YearOfRelease));
            });
        }

        //Create a new book with the same title but new author
        [Test]
        public async Task CreateBook_IfSameTitleNewAuthor_Return_Created()
        {
            _testBook = BookHelpers.CreateExistBook();
            _testBook.Author = "Test Author" + Guid.NewGuid();
            
            var response = await _libraryHttpService.CreateBook(_testBook);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var newBook = JsonConvert.DeserializeObject<Book>(jsonResult);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(jsonResult, Is.Not.Null);
                Assert.That(newBook.Title, Is.EqualTo(_testBook.Title));
                Assert.That(newBook.Author, Is.EqualTo(_testBook.Author));
                Assert.That(newBook.YearOfRelease, Is.EqualTo(_testBook.YearOfRelease));
            });
        }


        //Create a book with new title but the same author
        [Test]
        public async Task CreateBook_IfNewTitleSameAuthor_Return_Created()
        {
            _testBook = BookHelpers.CreateExistBook();
            _testBook.Title = "Test Book" + Guid.NewGuid();
            
            var response = await _libraryHttpService.CreateBook(_testBook);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var newBook = JsonConvert.DeserializeObject<Book>(jsonResult);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(jsonResult, Is.Not.Null);
                Assert.That(newBook.Title, Is.EqualTo(_testBook.Title));
                Assert.That(newBook.Author, Is.EqualTo(_testBook.Author));
                Assert.That(newBook.YearOfRelease, Is.EqualTo(_testBook.YearOfRelease));
            });
        }

        //Create a book with the same title and author
        [Test]
        public async Task CreateBook_IfSameTitleAndAuthor_Return_BadRequest()
        {
            _testBook = BookHelpers.CreateExistBook();
            
            var response = await _libraryHttpService.CreateBook(_testBook);
            var jsonResult = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }
    }
}
