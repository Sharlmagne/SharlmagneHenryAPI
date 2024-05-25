using System.Data;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using SharlmagneHenryAPI.Data;
using SharlmagneHenryAPI.Endpoints;
using SharlmagneHenryAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connString = builder.Configuration.GetConnectionString("AZURE_SQL_LOCAL");
builder.Services.AddSqlServer<DataContextEf>(connString);
builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(connString));
builder.Services.AddScoped<SkillService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error"); // This is the global exception handler
}

app.MapGet("/error", (HttpContext httpContext) =>
{
    var exceptionHandlerPathFeature = httpContext.Features.Get<IExceptionHandlerPathFeature>();
    var exception = exceptionHandlerPathFeature?.Error;

    // Log the exception, generate a correlation ID, do whatever you need to do

    return Results.Problem(
        title: "An unexpected error occurred!",
        detail: exception?.Message // Be careful with exposing exception details, don't do this in production
    );
});

// Register the ProjectsEndpoints
app.MapProjectsEndpoints();
app.MapSkillsEndpoints();

// Migrate the database
await app.MigrateDbAsync();

app.Run();