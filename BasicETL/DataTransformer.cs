using BasicETL.Models.Input;
using BasicETL.Models.Output;

namespace BasicETL;

public static class DataTransformer
{
    public static OutputData Transform(IList<InputDataRecord> records)
    {
        var outputData = new OutputData
        {
            Records = new List<OutputRecord>()
        };

        var cityNames = records.Select(r =>
        {
            var index = r.Address.IndexOf(',');
            r.Address = r.Address[..index];
            return r.Address;
        }).Distinct().ToList();

        foreach (var cityName in cityNames)
        {
            var cityRecords = records.Where(r => r.Address.StartsWith(cityName));
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

        return outputData;
    }
}