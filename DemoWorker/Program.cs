﻿using Communication;
using DemoWorker;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterServices();
builder.Services.HandleMessages();
builder.Services.HandleRequests();

var app = builder.Build();

app.HandleRequests();

app.Run();
