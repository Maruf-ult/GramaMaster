using GramaMaster.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.IO;

namespace GramaMaster.Infrastructure.Data
{
    public class GramaMasterDbContextFactory : IDesignTimeDbContextFactory<GramaMasterDbContext>
    {
        public GramaMasterDbContext CreateDbContext(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DbCon");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                // Fallback to appsettings.json path if available
                var appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "GramaMaster.API", "appsettings.json");
                if (!File.Exists(appSettingsPath))
                {
                    appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
                }

                if (File.Exists(appSettingsPath))
                {
                    var text = File.ReadAllText(appSettingsPath);
                    using var doc = System.Text.Json.JsonDocument.Parse(text);
                    if (doc.RootElement.TryGetProperty("ConnectionStrings", out var connSection) &&
                        connSection.TryGetProperty("DbCon", out var dbConProp))
                    {
                        connectionString = dbConProp.GetString();
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = "Host=localhost;Database=GramaMasterDb;Port=5432;Username=postgres;Password=maruf321";
            }

            var optionsBuilder = new DbContextOptionsBuilder<GramaMasterDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new GramaMasterDbContext(optionsBuilder.Options);
        }
    }
}

