using AwesomeGamingRacing.Data;
using AwesomeGamingRacing.Migrations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IDatabaseFactory, DatabaseFactory>();
builder.Services.AddScoped<IRaceRepository, RaceRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IImageManager, ImageManager>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Forbidden/";
        options.LoginPath = "/user/Login";
    });
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHttpsRedirection();
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

var cookiePolicyOptions = new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
};
app.UseCookiePolicy(cookiePolicyOptions);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Tracks",
    pattern: "{controller=Race}/{action=Track}/{name?}");

app.MapRazorPages();

IConfiguration configuration = app.Services.GetService<IConfiguration>();

createRaceDb();
void createRaceDb()
{
    string RacesDb = configuration["DatabaseNames:Race"];
    bool raceDoesExist = File.Exists(RacesDb);
    if (!raceDoesExist)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Races Does Not exist, creating...");
        var file = File.Create(RacesDb);
        file.Close();
        if (File.Exists(RacesDb))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");

            createUserDb();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("FAILED! ABORTING");
        }
    }
    else
    {
        createUserDb();
    }
}

void createUserDb()
{
    string UsersDb = configuration["DatabaseNames:User"];
    bool usersDoesExist = File.Exists(UsersDb);
    if (!usersDoesExist)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Users Does Not exist, creating...");
        var file = File.Create(UsersDb);
        file.Close();
        if (File.Exists(UsersDb))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");

            run();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("FAILED! ABORTING");
        }
    }
    else
    {
        run();
    }
}

void run()
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Migrations starting....");
    Console.ForegroundColor = ConsoleColor.White;

    Migrator migrator = new Migrator(app.Services.GetService<IDatabaseFactory>());
    migrator.RunRaceMigrations();
    migrator.RunUsersMigrations();

    app.Run();
}
