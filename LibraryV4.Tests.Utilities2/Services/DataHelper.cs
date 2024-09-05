using Bogus;
using LibraryV4.Contracts.Domain;

namespace LibraryV4.Tests.Services.Services
{
    public static class DataHelper
    {
        public static Book CreateBook()
        {
            var faker = new Faker();

            return new Book
            {
                Title = $"Pragmatic Programmer{faker.Random.AlphaNumeric(4)}",
                Author = "Andrew Hunt",
                YearOfRelease = 1999
            };
        }
    }
}
