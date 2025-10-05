var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.WebHost.UseUrls("http://localhost:5000");

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();