using Presentation;
using Infrastructure;

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

app.UseHttpsRedirection();
app.UseCors("AllowLocalhost"); 
app.UseAuthentication();
app.UseAuthorization();
app.UsePresentationLayer();

app.Run();
