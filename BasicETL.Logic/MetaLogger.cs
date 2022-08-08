using BasicETL.Logic.Models;

namespace BasicETL.Logic;

public class MetaLogger
{
    private readonly AppSettings _config;
    private readonly Meta _meta;
    private Timer? _timer;

    public MetaLogger(Meta meta, AppSettings config)
    {
        _meta = meta;
        _config = config;
    }

    public void StartLogTimer(CancellationToken cancellationToken)
    {
        var interval = TimeSpan.FromHours(24);
        var nextRunTime = DateTime.Today.AddHours(24).Subtract(TimeSpan.FromSeconds(10));
        var currentTime = DateTime.Now;
        var firstInterval = nextRunTime.Subtract(currentTime);

        Task.Run(() =>
        {
            var delay = Task.Delay(firstInterval, cancellationToken);
            delay.Wait(cancellationToken);

            LogMeta(null);

            _timer = new Timer(LogMeta, null, TimeSpan.Zero, interval);
        }, cancellationToken);
    }

    public void StopLogTimer()
    {
        _timer?.Dispose();
    }

    private void LogMeta(object? obj)
    {
        new FileWriter(_meta, _config.OutputFolder).WriteLog();
    }
}