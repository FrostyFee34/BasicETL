using CsvHelper.Configuration.Attributes;

namespace BasicETL.Logic.Models.Input;

public class InputDataRecord
{
    [Index(0)]
    public string? FirstName { get; set; }
    [Index(1)]
    public string? LastName { get; set; }
    [Index(2)]
    public string Address { get; set; }
    [Index(3)]
    public decimal Payment { get; set; }
    [Index(4)]
    [Format("yyyy-dd-mm")]
    public DateOnly Date { get; set; }
    [Index(5)]
    public long AccountNumber { get; set; }
    [Index(6)]
    public string Service { get; set; }

}