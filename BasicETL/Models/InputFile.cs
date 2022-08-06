namespace BasicETL.Models;

public class InputFile
{
    public string Name { get; set; }
    public string Extension { get; set; }
    public IList<InputDataLine> Lines { get; set; }
    public DateTime TimeOfEdit { get; set; }
}