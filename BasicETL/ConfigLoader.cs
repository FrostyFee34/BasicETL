using BasicETL.Models;

namespace BasicETL;

internal static class ConfigLoader
{
    public static Config? LoadConfig(string? configPath = null)
    {
        configPath ??= $"{Directory.GetCurrentDirectory()}\\etlConfig.cfg";

        try
        {
            using var sr = new StreamReader(configPath);
            
            var config = new Config
            {
                ObservableFolder = sr.ReadLine() ?? throw new InvalidOperationException(),
                OutputFolder = sr.ReadLine() ?? throw new InvalidOperationException()
            };
            return config;
        }
        catch (Exception e)
        {
            // TODO Logger
            return null;
        }
    }
}