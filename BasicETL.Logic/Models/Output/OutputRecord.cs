namespace BasicETL.Logic.Models.Output;

public class OutputRecord
{
    public string City { get; set; }
    public IList<Service> Services { get; set; }
    public decimal Total { get; set; }
}