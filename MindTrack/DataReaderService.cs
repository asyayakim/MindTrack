using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using MindTrack.Db;


namespace MindTrack;

public class DataReaderService
{
    private List<SocialAnxietyRecord> _cachedData;
    private readonly AppDbContext _context;

    public DataReaderService(string fileLocation, AppDbContext context )
    {
        FileLocation = fileLocation;
        _context = context;
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

     
    }

   

    public async Task<object> ImportLargeCsvAsync()
    {
        int totalInserted = 0;
        int batchSize = 1000;
        using var reader = new StreamReader(FileLocation);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var batch = new List<SocialAnxietyRecord>();
        await foreach (var record in csv.GetRecordsAsync<SocialAnxietyRecord>())
        {
            batch.Add(record);

            if (batch.Count >= batchSize)
            {
                _context.SocialAnxietyRecords.AddRange(batch);
                await _context.SaveChangesAsync();
                totalInserted += batch.Count;
                batch.Clear();
            }
        }
        if (batch.Count > 0)
        {
            _context.SocialAnxietyRecords.AddRange(batch);
            await _context.SaveChangesAsync();
            totalInserted += batch.Count;
        }
        return totalInserted;

    }
}