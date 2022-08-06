using BasicETL;

var config = ConfigLoader.LoadConfig(@"C:\Users\Aloha\Downloads\etlConfig.cfg");

if (config != null)
{
    var csvWatcher = new FileSystemWatcher(config.ObservableFolder, "*.csv");
    csvWatcher.EnableRaisingEvents = true;
    csvWatcher.Created += CsvWatcherOnCreated;

    var txtWatcher = new FileSystemWatcher(config.ObservableFolder, "*.txt");
    txtWatcher.EnableRaisingEvents = true;
    txtWatcher.Created += TxtWatcherOnCreated;

    new AutoResetEvent(false).WaitOne();
}

void TxtWatcherOnCreated(object obj, FileSystemEventArgs args)
{
    var thread = new Thread(() => Transformation(args, false));
    thread.Start();
}

void CsvWatcherOnCreated(object obj, FileSystemEventArgs args)
{
    var thread = new Thread(() => Transformation(args, true));
    thread.Start();
}

void Transformation(FileSystemEventArgs args, bool isCsv)
{
    var file = FileReader.ReadFile(args, isCsv);
    var outputData = DataTransformer.Transform(file.Records);
}