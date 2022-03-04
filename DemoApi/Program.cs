using Configuration;
using DemoApi;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddApiSource();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterServices();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
