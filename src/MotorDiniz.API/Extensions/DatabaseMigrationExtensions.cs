using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using MotorDiniz.Infra.Data.Context;

namespace MotorDiniz.API.Extensions
{
    public static class DatabaseMigrationExtensions
    {
        public static async Task ApplyMigrationsAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await db.Database.MigrateAsync();
        }
    }
}
