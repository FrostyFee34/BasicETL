namespace BasicETL.Models.Output;

public class Service
{
    public string Name { get; set; }
    public List<Payer> Payers { get; set; }
    public string Total { get; set; }
}