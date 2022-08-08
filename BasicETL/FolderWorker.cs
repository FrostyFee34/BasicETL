using BasicETL.Logic;
using BasicETL.Logic.Exceptions;
using BasicETL.Logic.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BasicETL.UI;

public class FolderWorker : BackgroundService
{
    private readonly ILogger<FolderWorker> _logger;
    private readonly IOptions<AppSettings> _settings;
    private FolderWatcher? _fw;

    public FolderWorker(IOptions<AppSettings> settings, ILogger<FolderWorker> logger)
    {
        _settings = settings;
        _logger = logger;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.CompletedTask;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var value = _settings.Value;
            if (string.IsNullOrEmpty(value.ObservableFolder) || string.IsNullOrEmpty(value.OutputFolder))
                throw new AppSettingsException();

            _fw = new FolderWatcher(_settings.Value, _logger);
            _fw.StartWatch(cancellationToken);
            return base.StartAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.Message);
            return Task.CompletedTask;
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _fw?.StopWatch();
        await base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}