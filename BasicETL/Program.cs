using BasicETL.Logic.Exceptions;
using BasicETL.Logic.Models;
using BasicETL.UI;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

var host = Host.CreateDefaultBuilder(args)
    .UseWindowsService((options) => { options.ServiceName = "BasicETL"; })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<FolderWorker>();
        services.Configure<AppSettings>(hostContext.Configuration.GetSection("AppSettings"));
    }).Build();

var config = host.Services.GetRequiredService<IOptions<AppSettings>>().Value;

if (config == null || string.IsNullOrEmpty(config.ObservableFolder) ||
    string.IsNullOrEmpty(config.OutputFolder))
{
    Console.WriteLine(new AppSettingsException().Message);
    return;
}

host.Run();