using System.Data;
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

// Register the ProjectsEndpoints
app.MapProjectsEndpoints();
app.MapSkillsEndpoints();

// Migrate the database
await app.MigrateDbAsync();

app.Run();
