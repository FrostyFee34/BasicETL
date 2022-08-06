namespace BasicETL.Models.Output;

public class OutputRecord
{
    public string City { get; set; }
    public List<Service> Services { get; set; }
    public string Total { get; set; }
}