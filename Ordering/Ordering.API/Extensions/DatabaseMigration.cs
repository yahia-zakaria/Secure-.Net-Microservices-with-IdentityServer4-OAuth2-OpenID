using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistance;
using Ordering.Infrastructure.Persistance;
using Polly;
using Serilog;
using System;
using System.Threading;

namespace Ordering.API.Extensions
{
    public static class DatabaseMigration
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<IUnitOfWork, IServiceProvider>
            seeder) where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetService<ILogger<TContext>>();
            var context = services.GetService<OrderContext>();
            var unitOfWork = services.GetService<IUnitOfWork>();
            try
            {
                logger.LogInformation("Migrate database associated with context {dbContext}", typeof(TContext).Name);

                //implement retry policy
                var retry = Policy
                    .Handle<SqlException>()
                    .WaitAndRetry(
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, timespan, context) =>
                    {
                        Log.Error($"Retry after {timespan.TotalMilliseconds} seconds due to {exception}");
                    });

                retry.Execute(()=> InvokeSeeder(seeder, context, unitOfWork, services));

                logger.LogInformation("Database associated with context {dbContext} has been migrated", typeof(TContext).Name);
            }
            catch (SqlException ex)
            {
                logger.LogError("An error occured while seeding database associated with context {ContextName} Exception message is: {message}", nameof(TContext), ex);
            }
            return host;
        }

        private static void InvokeSeeder(Action<IUnitOfWork, IServiceProvider> seeder, DbContext context,
            IUnitOfWork unitOfWork, IServiceProvider services)
        {
            context.Database.Migrate();
            seeder(unitOfWork, services);
        }
    }
}
