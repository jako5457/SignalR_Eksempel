using Microsoft.AspNetCore.Cors.Infrastructure;
using SignalR_Eksempel.Hubs;
using SignalR_Eksempel.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(config =>
{
    config.AddPolicy("allowAll", new CorsPolicyBuilder()
                                    .AllowAnyOrigin()
                                    .AllowAnyHeader()
                                    .AllowAnyMethod()
                                    .Build()
                                    );
});

builder.Services.AddScoped<ChatFilterService>();
builder.Services.AddSingleton<UserManager>();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors("allowAll");

app.UseAuthorization();

app.MapHub<ChatHub>("/Chat");

app.MapControllers();

app.Run();
