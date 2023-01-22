using System;
using System.Collections.Generic;
using System.Linq;

namespace test.Uteis;

public static class Generator
{
    public static string RandomString(int length = 10)
    {
        var random = new Random();
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(letters, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static int RandomInteger(int from = 1, int to = 10)
    {
        var random = new Random();
        return random.Next(from, to);
    }

    public static IEnumerable<int> RandomIntegerList(int from = 0, int to = 10, int length = 10)
    {
        var result = new List<int>();
        for (var i = 0; i < length; ++i)
        {
            result.Add(RandomInteger(from, to));
        }

        return result;
    }

    public static string GenerateIntegerString(int from = 0, int to = 10, int length = 10)
    {
        var list = RandomIntegerList(from, to, length);
        return string.Join("", list);
    }
}