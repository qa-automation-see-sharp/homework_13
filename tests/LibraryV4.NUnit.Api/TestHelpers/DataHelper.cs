﻿using LibraryV4.Contracts.Domain;

namespace LibraryV4.NUnit.Tests.Api.TestHelpers;

public class DataHelper
{
    public static Book CreateBook()
    {
        return new Book
        {
            Title = Guid.NewGuid().ToString(),
            Author = Guid.NewGuid().ToString(),
            YearOfRelease = new Random().Next(1850, 2024)
        };
    }
    public static Book CreateBook(string title, string author)
    {
        return new Book
        {
            Title = title,
            Author = author,
            YearOfRelease = new Random().Next(1900, 2024)
        };
    }
    public static User CreateUser()
    {
        return new User
        {
            NickName = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString(),
            FullName = Guid.NewGuid().ToString()
        };
    }
}