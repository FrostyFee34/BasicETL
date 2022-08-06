using System.Globalization;
using BasicETL.Models.Input;
using CsvHelper;
using CsvHelper.Configuration;

namespace BasicETL;

public class FileReader
{
    public static InputFile ReadFile(FileSystemEventArgs args, bool isCsv)
    {
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = isCsv
        };
        using var reader = new StreamReader(args.FullPath);
        using var csv = new CsvReader(reader, csvConfig);
        var inputFile = new InputFile()
        {
            Records = new List<InputDataRecord>(),
            Name = args.Name,
            TimeOfAddition = DateTime.Now
        };
        try
        {
            while (csv.Read())
            {
                var record = csv.GetRecord<InputDataRecord>();
                inputFile.Records.Add(record);
            }
            return inputFile;
        }
        catch (Exception e)
        {
            // TODO logger
            Console.WriteLine(e);
            throw;
        }

      
    }
}