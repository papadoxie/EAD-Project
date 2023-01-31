using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PUCCI.Data;
using PUCCI.Areas.Identity.Data;


var builder = WebApplication.CreateBuilder(args);

AddDbContext(builder);

ConfigureIdentity(builder);

ConfigureCookies(builder);

builder.Services.AddRazorPages();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

AddorUpdateDb(app);

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

void AddDbContext(WebApplicationBuilder builder)
{
    var connectionString = builder.Configuration.GetConnectionString("PUCCIContext") ?? throw new InvalidOperationException("Connection string 'PUCCIContextConnection' not found.");
    builder.Services.AddDbContext<PUCCIIdentityContext>(options => options.UseSqlServer(connectionString));
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

void AddorUpdateDb(WebApplication app)
{
    // Update DB with model changes
    var optionsBuilder = new DbContextOptionsBuilder<PUCCIIdentityContext>();
    optionsBuilder.UseSqlServer(
        app.Configuration.GetConnectionString("PUCCIContext")
    );
    var context = new PUCCIIdentityContext(optionsBuilder.Options);
    // context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
}
