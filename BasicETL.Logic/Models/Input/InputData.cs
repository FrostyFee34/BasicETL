namespace BasicETL.Logic.Models.Input;

public class InputData
{
    public string? FileName { get; set; }
    public List<InputDataRecord> Records { get; set; }
    public DateTime TimeOfAddition { get; set; }
}