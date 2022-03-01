using Microsoft.Extensions.Configuration;
using WebAdvert.SearchApi.Extensions;
using WebAdvert.SearchApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddElasticSearch(builder.Configuration);
builder.Services.AddTransient<ISearchService, SearchService>();

builder.Logging.AddAWSProvider(builder.Configuration.GetAWSLoggingConfigSection(),
                formatter: (loglevel, message, exception) => $"[{DateTime.Now} {loglevel} {message} {exception?.Message} {exception?.StackTrace}");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllOrigin", policy => policy.WithOrigins("*").AllowAnyHeader());
});


var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
