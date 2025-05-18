using JsonFx.Serialization;
using Microsoft.EntityFrameworkCore;
using MindTrack;
using MindTrack.Db;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var configuration = builder.Configuration;

builder.Services.AddScoped<DataReaderService>(provider =>
{
    var configuration = provider.GetService<IConfiguration>();
    var dbContext = provider.GetService<AppDbContext>();
    var fileLocation = configuration["AppSettings:DataFilePath"];
    return new DataReaderService(fileLocation, dbContext);
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();