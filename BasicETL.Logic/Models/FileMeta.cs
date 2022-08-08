namespace BasicETL.Logic.Models;

public class FileMeta
{
    public int ParsedLines { get; set; }
    public int FoundErrors { get; set; }
    public string? Path { get; set; }
}