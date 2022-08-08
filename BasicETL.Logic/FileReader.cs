using System.Globalization;
using BasicETL.Logic.Models;
using BasicETL.Logic.Models.Input;
using CsvHelper;
using CsvHelper.Configuration;

namespace BasicETL.Logic;

public class FileReader
{
    private readonly FileMeta _fileMeta = new();
    private readonly string _fullPath;
    private readonly bool _isCsv;
    private readonly Meta _meta;
    private readonly string _name;

    public FileReader(bool isCsv, string fullPath, string name, Meta meta)
    {
        _isCsv = isCsv;
        _fullPath = fullPath;
        _name = name;
        _meta = meta;
    }

    public async Task<InputData> ReadFile()
    {
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = _isCsv,
            TrimOptions = TrimOptions.Trim
        };
        using var reader = new StreamReader(_fullPath);
        using var csv = new CsvReader(reader, csvConfig);
        var inputFile = new InputData
        {
            Records = new List<InputDataRecord>(),
            FileName = _name,
            TimeOfAddition = DateTime.Now
        };

        while (await csv.ReadAsync())
        {
            _fileMeta.ParsedLines++;
            try
            {
                var record = csv.GetRecord<InputDataRecord>();
                if (string.IsNullOrEmpty(record.FirstName) && string.IsNullOrEmpty(record.LastName))
                    throw new InvalidOperationException(csv.CurrentIndex.ToString());
                inputFile.Records.Add(record);
            }
            catch
            {
                _fileMeta.FoundErrors++;
            }
        }

        if (inputFile.Records.Count <= 0) _fileMeta.Path = _fullPath;
        _meta.Log(_fileMeta);

        return inputFile;
    }
}