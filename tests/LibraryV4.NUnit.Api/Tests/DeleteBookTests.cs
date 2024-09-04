using System.Net;
using DocsForTests.TestHelpers;
using LibraryV4.Contracts.Domain;

namespace LibraryV4.NUnit.Api.Tests
{
    public class DeleteBookTests : Fixture
    {
        private Book Book { get; set; }

        [SetUp]
        public new async Task SetUp()
        {
            Book = DataHelper.BookHelper.RandomBook();
            await HttpService.PostBook(Book);
        }

        [Test]
        public async Task DeleteBook_OK()
        {
            var response = await HttpService.DeleteBook(Book.Title, Book.Author);
            var jsonString = await response.Content.ReadAsStringAsync();
            var s = jsonString.Trim('"');

            Assert.Multiple(() =>
            {
                Assert.That(s.Equals(DataHelper.ErrorMessage.DeleteBookReturnOK(Book)));
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        public async Task DeleteBook_NotFound()
        {
            var title = "Happy";
            var author = "Chan";
            var response = await HttpService.DeleteBook(title, author);
            var jsonString = await response.Content.ReadAsStringAsync();
            var s = jsonString.Trim('"');

            Assert.Multiple(() =>
            {
                Assert.That(s.Equals(DataHelper.ErrorMessage.DeleteBookNotFound(title, author)));
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public async Task DeleteBook_Unauthorized()
        {
            var response = await HttpService.DeleteBook(Book.Title, Book.Author, "123");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}