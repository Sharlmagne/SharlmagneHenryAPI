using SharlmagneHenryAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(
    (
        options =>
        {
            options.AddPolicy(
                "DevCorsPolicy",
                builder =>
                {
                    builder
                        .WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                }
            );
            options.AddPolicy(
                "ProdCorsPolicy",
                builder =>
                {
                    builder
                        .WithOrigins("http://productionwebsite.com")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                }
            );
        }
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors("ProdCorsPolicy");
    app.UseHttpsRedirection();
}

app.UseHttpsRedirection();

// Migrate the database
await app.MigrateDbAsync();

app.Run();