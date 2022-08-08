using BasicETL.Logic.Models.Output;
using Newtonsoft.Json;

namespace BasicETL.Logic;

public class FileWriter
{
    private readonly Meta _meta;
    private readonly string _outputPath;

    public FileWriter(Meta meta, string outputPath)
    {
        _meta = meta;
        _outputPath = outputPath;
    }

    private string DayPath => $@"{_outputPath}\{DateTime.Today:MM-dd-yyyy}";

    public async void WriteJson(OutputData outputData)
    {
        if (!Directory.Exists(DayPath))
            Directory.CreateDirectory(DayPath);
        await using var sr = File.CreateText(
            $@"{DayPath}\output{_meta.FilesTransformed}.json");
        var serializer = new JsonSerializer();
        serializer.Serialize(sr, outputData);
    }

    public async void WriteLog()
    {
        if (!Directory.Exists(DayPath))
            Directory.CreateDirectory(DayPath);
        await using var sr = File.CreateText($@"{DayPath}\meta.log");
        await sr.WriteAsync(_meta.ToString());
    }
}