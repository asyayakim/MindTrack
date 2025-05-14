using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;


namespace MindTrack;

public class DataReaderService
{
    private List<SocialAnxietyRecord> _cachedData;

    public DataReaderService(string fileLocation)
    {
        FileLocation = fileLocation;
    }
    public string FileLocation { get; init; }

    public async Task<(List<SocialAnxietyRecord> data, int totalCount)> ReadDataAsync(int page, int pageSize)
    {
        if (_cachedData == null)
        {
            _cachedData = await ReadDataAsync();
        }

        var totalCount = _cachedData.Count;
        var paginatedData = _cachedData
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return (paginatedData, totalCount);
    }

    private async Task<List<SocialAnxietyRecord>> ReadDataAsync()
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            MissingFieldFound = null,
            BufferSize = 4096 * 16,
            BadDataFound = (context) => { Console.WriteLine($"Bad data found on row: {context.RawRecord}"); }
        };
        var allRecords = new List<SocialAnxietyRecord>();
        var currentBatch = new List<SocialAnxietyRecord>();
        using var reader = new StreamReader(FileLocation);
        using var csv = new CsvReader(reader, config);

        await foreach (var record in csv.GetRecordsAsync<SocialAnxietyRecord>())
        {
            allRecords.Add(record);
            if (currentBatch.Count % 1000 == 0)
            {
                await ProcessBatchAsync(allRecords.Skip(allRecords.Count - 1000).ToList());
            }
        }
        if (allRecords.Count % 1000 != 0)
        {
            await ProcessBatchAsync(allRecords.Skip(allRecords.Count - (allRecords.Count % 1000)).ToList());
        }

        return allRecords;
    }

    private async Task ProcessBatchAsync(List<SocialAnxietyRecord> batch)
    {
        if (batch == null || batch.Count == 0)
        {
            Console.WriteLine("Received an empty batch. Nothing to process.");
            return;
        }

        Console.WriteLine($"Processing batch of {batch.Count} records...");
       
    }
}