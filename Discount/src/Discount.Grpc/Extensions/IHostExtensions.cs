using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data;
using Dapper;
using System;

namespace Discount.Grpc.Extensions
{
    public static class IHostExtensions
    {

        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            var retryForAvailability = retry.Value;
            using var scope = host.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var logger = serviceProvider.GetRequiredService<ILogger<TContext>>(); 
            var conn = serviceProvider.GetRequiredService<IDbConnection>();

            conn.Open();
            logger.LogInformation("Start database migration....");
            var tableExists = conn.ExecuteScalar<bool>("Select EXISTS(SELECT FROM pg_tables WHERE schemaname=@schemaName AND tablename=@TableName)",
                new { schemaName = "public", tableName = "coupon" });

            if(!tableExists)
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
                    conn.Execute(sql, new { productName  = "IPhone X", desc = "IPhone Discount", amount = 150});
                    conn.Execute(sql, new { productName  = "Samsung 10", desc = "Samsung Discount", amount = 100});

                    logger.LogInformation("Finishing database migration....");
                }
                catch (Exception)
                {
                    logger.LogError("Error while migrating database....");
                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, retryForAvailability);
                    }
                }
                finally
                {
                    conn.Close();
                }
            }

            return host;
        }
    }
}
