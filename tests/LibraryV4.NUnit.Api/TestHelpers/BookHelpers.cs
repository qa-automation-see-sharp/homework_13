using LibraryV4.Contracts.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryV4.NUnit.Api.TestHelpers
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
