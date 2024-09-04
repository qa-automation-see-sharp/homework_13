using System.Net;
using DocsForTests.TestHelpers;
using LibraryV4.Contracts.Domain;
using Newtonsoft.Json;

namespace LibraryV4.NUnit.Api.Tests
{
    public class CreateBookTests : Fixture
    {
        private Book _book;

        [TestCase("Philosopher's Stone", "Joanne Rowling", 1997)]
        [TestCase("Chamber of Secrets", "Joanne Rowling", 1998)]
        [TestCase("Prisoner of Azkaban", "Joanne Rowling", 1999)]
        [TestCase("Goblet of Fire ", "Joanne Rowling", 2000)]
        [TestCase("Order of the Phoenix", "Joanne Rowling", 2003)]
        [TestCase("Half-Blood Prince", "Joanne Rowling", 2005)]
        public async Task CreateBook_ReturnCreated(string title, string author, int year)
        {
            _book = DataHelper.BookHelper.BookWithTitleAuthorYear(title, author, year);

            var obj = await HttpService.PostBook(_book);
            var response = await obj.Content.ReadAsStringAsync();
            var bookObj = JsonConvert.DeserializeObject<Book>(response);

            Assert.Multiple(() =>
            {
                Assert.That(obj.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(response, Is.Not.Null);
                Assert.That(bookObj.Title, Is.EqualTo(_book.Title));
                Assert.That(bookObj.Author, Is.EqualTo(_book.Author));
                Assert.That(bookObj.YearOfRelease, Is.EqualTo(_book.YearOfRelease));
            });
        }

        [Test]
        public new async Task CreateExistedBook_ReturnBadRequest()
        {
            _book = DataHelper.BookHelper.RandomBook();

            await HttpService.PostBook(_book);
            var obj = await HttpService.PostBook(_book);
            var response = await obj.Content.ReadAsStringAsync();
            var s = response.Trim('"');

            Assert.Multiple(() =>
           {
               Assert.That(obj.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
               Assert.That(response, Is.Not.Null);
               Assert.That(s.Equals(DataHelper.ErrorMessage.ExistBook(_book)));
           });
        }

        [TearDown]
        public new async Task DeleteBook()
        {
            TestContext.WriteLine("Tear down:");
            await HttpService.DeleteBook(_book.Title, _book.Author);
        }
    }
}