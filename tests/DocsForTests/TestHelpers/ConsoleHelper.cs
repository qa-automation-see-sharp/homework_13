using System;

namespace DocsForTests.TestHelpers;

public class ConsoleHelper
{
    public static void Info(HttpMethod methodHTTP, string name, string url, string jsonString, HttpResponseMessage response)
    {
        Console.WriteLine($"{name}:");
        Console.WriteLine($"{methodHTTP} request to: {url}");
        Console.WriteLine($"Content: {jsonString}");
        Console.WriteLine($"Response status code is : {response.StatusCode}");
    }

    public static void SetUpFromClass(string nameClass)
    {
        Console.WriteLine($"Set Up from class {nameClass}");
    }

    public static void TearDownFromClass(string nameClass)
    {
        Console.WriteLine($"Tear Down from class {nameClass}");
    }
}
