using Microsoft.EntityFrameworkCore;

namespace SharlmagneHenryAPI.Data;

public static class DataExtensions
{
    // MigrateDbAsync method is an extension method that can be called on a WebApplication object.
    // This method is used to apply any pending migrations to the database.
    public static async Task MigrateDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContextEf>();
        await dbContext.Database.MigrateAsync();
    }
}