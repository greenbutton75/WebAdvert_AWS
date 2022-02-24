using WebAdvert.Web.ServiceClients;
using WebAdvert.Web.Services;
using Polly;
using Polly.Extensions.Http;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddCognitoIdentity(config =>
{
    config.Password = new Microsoft.AspNetCore.Identity.PasswordOptions
    {
        RequireDigit = true,
        RequiredLength = 8,
        RequiredUniqueChars = 0,
        RequireLowercase = false,
        RequireNonAlphanumeric = false,
        RequireUppercase = false
    };
});

builder.Services.ConfigureApplicationCookie(configure =>
{
    configure.LoginPath = "/Accounts/Login";
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<IFileUploader, S3FileUploader>();

builder.Services.AddHttpClient<IAdvertApiClient, AdvertApiClient>();//.AddPolicyHandler(GetRetryPolicy()).AddPolicyHandler(GetCircuitBreakerPatternPolicy());
builder.Services.AddHttpClient<ISearchApiClient, SearchApiClient>();//.AddPolicyHandler(GetRetryPolicy()).AddPolicyHandler(GetCircuitBreakerPatternPolicy());


IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPatternPolicy()
{
    return HttpPolicyExtensions.HandleTransientHttpError().CircuitBreakerAsync(3, TimeSpan.FromSeconds(30));
}

IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions.HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
   pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
