using BasicETL.Logic.Models.Input;
using BasicETL.Logic.Models.Output;

namespace BasicETL.Logic;

public class DataTransformer
{
    private readonly InputData _inputData;
    private readonly Meta _meta;

    public DataTransformer(InputData inputData, Meta meta)
    {
        _inputData = inputData;
        _meta = meta;
    }

    private IEnumerable<InputDataRecord> Records => _inputData.Records;

    public OutputData Transform()
    {
        var outputData = new OutputData
        {
            Records = new List<OutputRecord>()
        };

        var cityNames = Records.Select(r =>
        {
            var index = r.Address.IndexOf(',');
            r.Address = r.Address[..index];
            return r.Address;
        }).Distinct().ToList();

        foreach (var cityName in cityNames)
        {
            var cityRecords = Records.Where(r => r.Address.StartsWith(cityName));
            var cityTotal = cityRecords.Select(r => r.Payment)
                .Aggregate(decimal.Add);
            var cityServices = new List<Service>();
            var cityServiceNames = cityRecords.Select(r => r.Service).Distinct();

            foreach (var cityServiceName in cityServiceNames)
            {
                var cityServiceRecords = cityRecords.Where(r => r.Service == cityServiceName);
                var cityServiceTotal = cityServiceRecords.Select(r => r.Payment).Aggregate(decimal.Add);
                var cityServicePayers = new List<Payer>();
                var cityServicePayerNames = cityServiceRecords.Select(r => $"{r.FirstName} {r.LastName}").Distinct();

                foreach (var cityServicePayerName in cityServicePayerNames)
                {
                    var cityServicePayerRecord =
                        cityServiceRecords.First(r => cityServicePayerName == $"{r.FirstName} {r.LastName}");

                    var payer = new Payer
                    {
                        Name = cityServicePayerName,
                        AccountNumber = cityServicePayerRecord.AccountNumber,
                        Date = cityServicePayerRecord.Date,
                        Payment = cityServicePayerRecord.Payment
                    };
                    cityServicePayers.Add(payer);
                }

                var cityService = new Service
                {
                    Name = cityServiceName,
                    Payers = cityServicePayers,
                    Total = cityServiceTotal
                };
                cityServices.Add(cityService);
            }

            var outputRecord = new OutputRecord
            {
                City = cityName,
                Total = cityTotal,
                Services = cityServices
            };
            outputData.Records.Add(outputRecord);
        }

        _meta.FileTransformed();
        return outputData;
    }
}