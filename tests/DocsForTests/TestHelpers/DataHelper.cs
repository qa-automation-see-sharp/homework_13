using System;
using LibraryV4.Contracts.Domain;

namespace DocsForTests.TestHelpers;

public class DataHelper
{
    public static class BookHelper
    {
        public static Book RandomBook()
        {
            return new()
            {
                Title = Guid.NewGuid().ToString(),
                Author = Guid.NewGuid().ToString(),
                YearOfRelease = new Random().Next(1800, 2024)
            };
        }

        public static Book BookWithTitleAuthorYear(string title, string author, int yearOfRealease)
        {
            return new()
            {
                Title = title,
                Author = author,
                YearOfRelease = yearOfRealease
            };
        }
    }

    public static class UserHelper
    {
        public static User CreateRandomUser()
        {
            return new()
            {
                NickName = Guid.NewGuid().ToString(),
                FullName = Guid.NewGuid().ToString(),
                Password = new Random().Next().ToString()
            };
        }

        public static User CreateUser(string nickName, string fullName, string password)
        {
            return new()
            {
                NickName = nickName,
                FullName = fullName,
                Password = password
            };
        }
    }

    public static class ErrorMessage
    {
        public static string InvalidLogin = "Invalid nickname or password";
        public static string ExistUser(string nickName) => $"User with nickname {nickName} already exists";
        public static string ExistBook(Book book) => $"{book.Title} by {book.Author}, {book.YearOfRelease} already exists";
        public static string NotFoundBookByTitle(Book book) => $"The books this title: {book.Title}, was not found.";
        public static string DeleteBookReturnOK(Book book) => $"{book.Title} by {book.Author} deleted";
        public static string DeleteBookNotFound(string title, string author) => $"Book :{title} by {author} not found";
    }
}
