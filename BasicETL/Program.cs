using BasicETL;
using BasicETL.Models;

var config = LoadConfig(@"C:\Users\Aloha\Downloads\etlConfig.cfg");

Config? LoadConfig(string? configPath = null)
{
    configPath ??= $"{Directory.GetCurrentDirectory()}\\etlConfig.cfg";

    try
    {
        var configReader = new FileReader(configPath);
        var lines = configReader.Read().ToArray();
        if (lines.Length == 0) throw new InvalidOperationException();
        var config = new Config
        {
            ObservableFolder = lines[0] ?? throw new InvalidOperationException(),
            OutputFolder = lines[1] ?? throw new InvalidOperationException()
        };
        return config;
    }
    catch (Exception e)
    {
        // TODO Logger
        return null;
    }

}