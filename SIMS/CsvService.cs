// CsvService.cs
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using System.IO;

public static class CsvService
{
    public static List<T> LoadFromCsv<T>(string filePath)
    {
        if (!File.Exists(filePath)) return new List<T>();

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<T>().ToList();
    }

    public static void SaveToCsv<T>(List<T> data, string filePath)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(data);
    }
}
