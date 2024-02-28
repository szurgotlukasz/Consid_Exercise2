using Exercise2.Controllers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(x => x.AddConsole());
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("WeatherDb"));
builder.Services.AddHttpClient();
builder.Services.AddHostedService<WeatherBackgroundService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseRouting();
app.MapControllers();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");
app.Run();