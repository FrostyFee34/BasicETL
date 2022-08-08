using BasicETL.Logic.Models;
using Microsoft.Extensions.Logging;

namespace BasicETL.Logic;

public class FolderWatcher : IDisposable
{
    private readonly AppSettings _config;
    private readonly ILogger _logger;
    private readonly Meta _meta;
    private FileSystemWatcher? _csvWatcher;
    private MetaLogger? _metaLogger;
    private FileSystemWatcher? _txtWatcher;

    public FolderWatcher(AppSettings config, ILogger logger)
    {
        _config = config;
        _logger = logger;
        _meta = new Meta();
    }

    private string ObservableFolder => _config.ObservableFolder;

    public void Dispose()
    {
        _csvWatcher?.Dispose();
        _txtWatcher?.Dispose();
    }


    private async void CsvWatcherOnCreated(object sender, FileSystemEventArgs e)
    {
        await Task.Run(() =>
        {
            if (e.Name != null) RunTransformation(true, e.FullPath, e.Name);
        });
    }

    private async void TxtWatcherOnCreated(object sender, FileSystemEventArgs e)
    {
        await Task.Run(() =>
        {
            if (e.Name != null) RunTransformation(false, e.FullPath, e.Name);
        });
    }

    public void StopWatch()
    {
        _metaLogger?.StopLogTimer();
        if (_txtWatcher != null) _txtWatcher.EnableRaisingEvents = false;
        if (_csvWatcher != null) _csvWatcher.EnableRaisingEvents = false;
        _logger.LogInformation($"\n{_meta}");
    }

    public void StartWatch(CancellationToken cancellationToken)
    {
        try
        {
            _metaLogger = new MetaLogger(_meta, _config);
            _metaLogger.StartLogTimer(cancellationToken);

            _txtWatcher = new FileSystemWatcher(ObservableFolder, "*.txt");
            _csvWatcher = new FileSystemWatcher(ObservableFolder, "*.csv");

            _csvWatcher.EnableRaisingEvents = true;
            _txtWatcher.EnableRaisingEvents = true;

            _txtWatcher.Created += TxtWatcherOnCreated;
            _csvWatcher.Created += CsvWatcherOnCreated;
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.Message);
            _logger.LogTrace(e.StackTrace);
        }
    }

    private async void RunTransformation(bool isCsv, string fullPath, string name)
    {
        try
        {
            var inputData = await new FileReader(isCsv, fullPath, name, _meta).ReadFile();
            if (inputData.Records.Count <= 0) return;
            var outputData = await Task.Run(() => new DataTransformer(inputData, _meta).Transform());
            new FileWriter(_meta, _config.OutputFolder).WriteJson(outputData);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            _logger.LogTrace(e.StackTrace);
        }
    }
}