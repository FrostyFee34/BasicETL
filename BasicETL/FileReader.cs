using BasicETL.Models;

namespace BasicETL;

public class FileReader
{
    private readonly string _pathToFile;

    public FileReader(string pathToFile)
    {
        _pathToFile = pathToFile;
    }

    public IEnumerable<string?> Read()
    {

        using var sr = new StreamReader(_pathToFile);
        while (!sr.EndOfStream)
        { 
            yield return sr.ReadLine();
        }
    }
    
}