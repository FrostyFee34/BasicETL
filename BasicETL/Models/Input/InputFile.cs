namespace BasicETL.Models.Input;

public class InputFile
{
    public string? Name { get; set; }
    public List<InputDataRecord> Records { get; set; }
    public DateTime TimeOfAddition { get; set; }
}