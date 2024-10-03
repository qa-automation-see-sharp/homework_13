using LibraryV4.Contracts.Domain;

//TODO fix namespace
namespace LibraryV4.xUnit.Tests.Api.Tests.TestHelpers
{
    public static class BookHelpers
    {
        public static Book CreateBook()
        {
            return new Book
            {
                Title = "Test Book" + Guid.NewGuid().ToString(),
                Author = "Test Author" + Guid.NewGuid().ToString(),
                YearOfRelease = new Random().Next(1900, 2022)
            };
        }

        public static Book CreateBook(string title, string author)
        {
            return new Book
            {
                Title = title,
                Author = author,
                YearOfRelease = new Random().Next(1900, 2022)
            };
        }

        public static Book CreateExistBook()
        {
            return new Book
            {
                Title = "Test Book",
                Author = "Test Author",
                YearOfRelease = 2021
            };
        }
        
    }
}
