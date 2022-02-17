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
    configure.LoginPath = "Accounts/Login";
});


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
