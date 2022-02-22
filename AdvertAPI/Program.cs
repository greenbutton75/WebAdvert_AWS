using AdvertAPI.Services;
using Amazon.DynamoDBv2;
using AdvertAPI.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<AmazonDynamoDBClient>();
builder.Services.AddScoped<IAdvertStorageService, DynamoDBAdvertStorage>();

builder.Services.AddHealthChecks().AddCheck<StorageHealthCheck>("Storage");

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
app.UseHealthChecks("/health");

app.UseAuthorization();

app.MapControllers();

app.Run();
