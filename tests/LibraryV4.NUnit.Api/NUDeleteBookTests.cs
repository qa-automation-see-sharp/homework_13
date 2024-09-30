using LibraryV4.Contracts.Domain;
using LibraryV4.Services;
using LibraryV4.NUnit.Api.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibraryV4.NUnit.Tests.Api.Tests
{
    public class DeleteBookTests
    {
        private Guid _toke = Guid.Empty;
        private Book _testBook;
        private LibraryHttpService _libraryHttpService = new();

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            _libraryHttpService = new LibraryHttpService();
            await _libraryHttpService.CreateTestUser();
            await _libraryHttpService.LoginTestUser();
            
        }
        [SetUp]
        public async Task SetUp()
        {
            await _libraryHttpService.CreateTestBook();
        }

        //Delete book if book exist in library return Ok
        [Test]
        public async Task DeleteBook_IfExistInLibrary_return_Ok()
        {
            _testBook = BookHelpers.CreateExistBook();

            HttpResponseMessage response = await _libraryHttpService.DeleteBook(_testBook.Title, _testBook.Author);
            var json = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(json, Is.Not.Null);

            });
        }

        //Delete book if book does not exist return not found
        [Test]
        public async Task DeleteBook_IfNotExistInLibrary_return_NotFound()
        {
            _testBook = BookHelpers.CreateBook();
            
            HttpResponseMessage response = await _libraryHttpService.DeleteBook(_testBook.Title, _testBook.Author);
            var json = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                Assert.That(json, Is.Not.Null);

            });

        }
        
    }
}

