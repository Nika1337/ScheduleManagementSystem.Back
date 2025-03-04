using Presentation;
using Infrastructure;
using Infrastructure.Data;
using Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);
builder.Services.AddPresentationLayer(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:5173") 
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); 
    });
});

var app = builder.Build();

app.ConfigureExceptionHandler();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await DataSeeder.SeedAsync(serviceProvider);
}


app.UseHttpsRedirection();
app.UseCors("AllowLocalhost"); 
app.UsePresentationLayer();

app.Run();
