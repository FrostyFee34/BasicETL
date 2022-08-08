using BasicETL.Logic.Models;

namespace BasicETL.Logic;

public class Meta
{
    private int ParsedLines { get; set; }
    public int FilesTransformed { get; private set; }
    private int ParsedFiles { get; set; }
    private int FoundErrors { get; set; }
    private List<string> InvalidFiles { get; } = new();

    public void Log(FileMeta fileMeta)
    {
        if (fileMeta.Path != null) InvalidFiles.Add(fileMeta.Path);
        FoundErrors += fileMeta.FoundErrors;
        ParsedLines += fileMeta.ParsedLines;
        ParsedFiles++;
    }

    public void FileTransformed()
    {
        FilesTransformed++;
    }
    public override string ToString()
    {
        return $"parsed_files: {ParsedFiles}\n" +
               $"parsed_lines: {ParsedLines}\n" +
               $"found_errors: {FoundErrors}\n" +
               $"invalid_files:[{string.Join(", ", InvalidFiles)}]";
    }
}