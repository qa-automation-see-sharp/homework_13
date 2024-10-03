using System.Net;
using Newtonsoft.Json;
using LibraryV4.Contracts.Domain;
using LibraryV4.Services;
using LibraryV4.NUnit.Api.TestHelpers;

//TODO fix namespace
namespace LibraryV4.NUnit.Tests.Api.Tests
{
    public class GetBookTests
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

        [SetUp]
        public async Task SetUp()
        {

        }

        //Get Book by Title If title exist return Ok
        [Test]
        public async Task GetBookByTitle_IfTitleExist_Return_Ok()
        {
            _testBook = BookHelpers.CreateExistBook();

            HttpResponseMessage response = await _libraryHttpService.GetBooksByTitle(_testBook.Title);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var books = JsonConvert.DeserializeObject<List<Book>>(jsonResult);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }

        //Get Book by title If title does not exist return Not Found
        [Test]
        public async Task GetBookByTitle_IfTitleNotExist_Return_NotFound()
        {
            _testBook = BookHelpers.CreateBook();

            HttpResponseMessage response = await _libraryHttpService.GetBooksByTitle(_testBook.Title);
            var jsonResult = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }

        //Get Book By Author if Author exist return Ok
        [Test]
        public async Task GetBookByAuthor_IfAuthorExist_Return_Ok()
        {
            _testBook = BookHelpers.CreateExistBook();

            HttpResponseMessage response = await _libraryHttpService.GetBooksByAuthor(_testBook.Author);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var books = JsonConvert.DeserializeObject<List<Book>>(jsonResult);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }

        //Get Book by Author if Author does not exist return Not Foud
        [Test]
        public async Task GetBookByAuthor_IfAuthorNotExist_Return_NotFound()
        {
            _testBook = BookHelpers.CreateBook();

            HttpResponseMessage response = await _libraryHttpService.GetBooksByAuthor(_testBook.Author);
            var jsonResult = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }

    }
}
