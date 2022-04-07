using Communication;
using Configuration;
using DemoWorker;

Monitoring.Registration.StartMetrics(3000);
var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddApiSource();
builder.Services.RegisterServices();
builder.Services.HandleMessages();
builder.Services.HandleRequests();

var app = builder.Build();

app.HandleRequests();

app.Run();

