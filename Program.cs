using SharlmagneHenryAPI.Data;
using SharlmagneHenryAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connString = builder.Configuration.GetConnectionString("AZURE_SQL_LOCAL");
builder.Services.AddSqlServer<DataContextEf>(connString);

var app = builder.Build();

// Register the ProjectsEndpoints
app.MapProjectsEndpoints();
app.MapSkillsEndpoints();

// Migrate the database
await app.MigrateDbAsync();

app.Run();
