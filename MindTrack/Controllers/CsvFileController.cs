using Microsoft.AspNetCore.Mvc;

namespace MindTrack.Controllers;

[ApiController]
[Route("[controller]")]
public class CsvFileController : ControllerBase
{
    private readonly DataReaderService _data;

    public CsvFileController(DataReaderService data)
    {
        _data = data;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllData( [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var result = await _data.ReadDataAsync(page, pageSize);;
            return Ok(new
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = result.totalCount,
                TotalPages = (int)Math.Ceiling(result.totalCount / (double)pageSize),
                Data = result.data
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}