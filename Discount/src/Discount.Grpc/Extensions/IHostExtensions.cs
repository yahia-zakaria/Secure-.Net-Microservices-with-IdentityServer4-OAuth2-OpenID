using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data;
using Dapper;
using System;
using Polly;
using Npgsql;
using Serilog;

namespace Discount.Grpc.Extensions
{
    public static class IHostExtensions
    {

        public static IHost MigrateDatabase<TContext>(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var logger = serviceProvider.GetRequiredService<ILogger<TContext>>();
            var conn = serviceProvider.GetRequiredService<IDbConnection>();

            //implement retry pattern
            var retry = Policy
        .Handle<NpgsqlException>()
        .WaitAndRetry(
        retryCount: 5,
        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
        onRetry: (exception, timespan, context) =>
        {
            Log.Error($"Retry after {timespan.TotalMilliseconds} seconds due to {exception}");
        });

            retry.Execute(() => ExecutingMigration(logger, conn));

            return host;
        }

        private static void ExecutingMigration<TContext>(ILogger<TContext> logger, IDbConnection conn)
        {
            conn.Open();
            logger.LogInformation("Start database migration....");
            var tableExists = conn.ExecuteScalar<bool>("Select EXISTS(SELECT FROM pg_tables WHERE schemaname=@schemaName AND tablename=@TableName)",
                new { schemaName = "public", tableName = "coupon" });

            if (!tableExists)
            {
                try
                {
                    var ddl = "create table Coupon(" +
                              "ID serial primary key not null," +
                              "ProductName varchar(24) not null," +
                              "Description text," +
                              "Amount int" +
                              ")";
                    conn.Execute(ddl);

                    var sql = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES(@productName, @desc, @amount)";
                    conn.Execute(sql, new { productName = "IPhone X", desc = "IPhone Discount", amount = 150 });
                    conn.Execute(sql, new { productName = "Samsung 10", desc = "Samsung Discount", amount = 100 });

                    logger.LogInformation("Finishing database migration....");
                }
                catch (NpgsqlException ex)
                {
                    logger.LogError("Error while migrating database due to {ex}", ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
