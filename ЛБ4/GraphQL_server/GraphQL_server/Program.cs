using GraphQL_server;
using GraphQL_server.Controllers;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGraphQLServer()
    .AddQueryType<Query>();

var app = builder.Build();

app.MapGraphQL();
app.Run();
