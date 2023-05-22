using AwesomeGamingRacing.Data;
using AwesomeGamingRacing.Migrations;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IDatabaseFactory, DatabaseFactory>();
builder.Services.AddScoped<IRaceRepository, RaceRepository>();
builder.Services.AddSingleton<IImageManager, ImageManager>();

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

app.UseAuthorization();

app.MapGet("/", () => "Hello ForwardedHeadersOptions!");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Tracks",
    pattern: "{controller=Race}/{action=Track}/{name?}");

app.MapRazorPages();

IConfiguration configuration = app.Services.GetService<IConfiguration>();

if(!File.Exists("/" + configuration["DatabaseNames:Race"]))
{
    var file = File.Create("/" + configuration["DatabaseNames:Race"]);
    file.Close();
}

Migrator migrator = new Migrator(app.Services.GetService<IDatabaseFactory>());
migrator.RunUp();

app.Run();
