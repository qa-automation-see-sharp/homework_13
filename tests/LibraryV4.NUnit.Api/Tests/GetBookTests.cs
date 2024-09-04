using System.Net;
using DocsForTests.TestHelpers;
using LibraryV4.Contracts.Domain;
using Newtonsoft.Json;

namespace LibraryV4.NUnit.Api.Tests
{
    public class GetBookTests : Fixture
    {
        private Book Book { get; set; }

        [SetUp]
        public new async Task SetUp()
        {
            Book = DataHelper.BookHelper.RandomBook();
            await HttpService.PostBook(Book);
        }

        [Test]
        public async Task GetBooksByTitle()
        {
            var response = await HttpService.GetBooksByTitle(Book.Title);
            var listStringBooks = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<List<Book>>(listStringBooks);

            Assert.Multiple(() =>
            {
                Assert.That(response.IsSuccessStatusCode, Is.True);
                Assert.That(json, Is.Not.Empty);
                Assert.That(response, Is.Not.Null);
                Assert.That(json[0].Title, Is.EqualTo(Book.Title));
                Assert.That(json[0].Author, Is.EqualTo(Book.Author));
                Assert.That(json[0].YearOfRelease, Is.EqualTo(Book.YearOfRelease));
            });
        }

        [Test]
        public async Task BookNotFoundByTitle()
        {

            var book = DataHelper.BookHelper.BookWithTitleAuthorYear("Not Found", "Not Found Author", 1990);
            var response = await HttpService.GetBooksByTitle(book.Title);
            var message = await response.Content.ReadAsStringAsync();
            var s = message.Trim('"');

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                Assert.That(s.Equals(DataHelper.ErrorMessage.NotFoundBookByTitle(book)));
            });
        }

        [Test]
        public async Task GetBooksByAuthor()
        {
            var response = await HttpService.GetBooksByAuthor(Book.Author);
            var listStringBooks = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<List<Book>>(listStringBooks);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response, Is.Not.Null);
                Assert.That(json[0].Title, Is.EqualTo(Book.Title));
                Assert.That(json[0].Author, Is.EqualTo(Book.Author));
                Assert.That(json[0].YearOfRelease, Is.EqualTo(Book.YearOfRelease));
            });
        }
    }
}