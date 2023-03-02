using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using PUCCI.Data;
using PUCCI.Areas.Identity.Data;
using PUCCI.Models.Audit;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

ConfigureDataProtection(builder);

ConfigureModel(builder);

ConfigureIdentity(builder);

ConfigureCookies(builder);

builder.Services.AddRazorPages();

var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapRazorPages();

app.Run();

void ConfigureDataProtection(WebApplicationBuilder builder)
{
    builder.Services.AddDataProtection().UseCryptographicAlgorithms(
        new AuthenticatedEncryptorConfiguration
        {
            EncryptionAlgorithm = EncryptionAlgorithm.AES_128_CBC,
            ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
        });
}

void ConfigureModel(WebApplicationBuilder builder)
{
    var connectionString = builder.Configuration.GetConnectionString("PUCCIContext");
    if (connectionString == null)
    {
        builder.Configuration.AddAzureKeyVault(
            new Uri(builder.Configuration["KeyVaultConfig:KvURL"]!),
            new ClientSecretCredential(
                builder.Configuration["KeyVaultConfig:TenantId"],
                builder.Configuration["KeyVaultConfig:ClientId"],
                builder.Configuration["KeyVaultConfig:ClientSecret"]
            )
        );
    }
    builder.Services.AddDbContext<PUCCIIdentityContext>(options => options.UseSqlServer(connectionString));
    builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();
}

void ConfigureIdentity(WebApplicationBuilder builder)
{
    builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<PUCCIIdentityContext>();
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    builder.Services.Configure<IdentityOptions>(options =>
    {
        // Password settings.
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 2;
        options.Password.RequiredUniqueChars = 1;

        // Lockout settings.
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        // User settings.
        options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true;
    });
}

void ConfigureCookies(WebApplicationBuilder builder)
{
    builder.Services.ConfigureApplicationCookie(options =>
    {
        // Cookie settings
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

        options.LoginPath = "/Identity/Account/Login";
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        options.SlidingExpiration = true;
    });
}