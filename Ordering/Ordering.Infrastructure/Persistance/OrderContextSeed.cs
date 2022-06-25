using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistance;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistance
{
    public class OrderContextSeed
    {
        public async static Task SeedAsync(IUnitOfWork unitOfwork, ILogger<OrderContextSeed> logger)
        {
            if (!await unitOfwork.Orders.AnyAsync())
            {
                await unitOfwork.Orders.AddRangeAsync(preconfiuredOrder());
                await unitOfwork.SaveChangesAsync();
                logger.LogInformation("Seed database associated with {contextName}", typeof(OrderContext));
            }
        }

        private static List<Order> preconfiuredOrder()
        {
            List<Order> orders = new() { new()
            {
                AddressLine = "address",
                CardName = "yahia zakaria",
                CardNumber = "345688906678986",
                Country = "sudan",
                CVV = "444",
                EmailAddress = "yahia@hotmail.com",
                FirstName = "yahia",
                LastName = "zakaria"
            ,
                Expiration = "0723",
                PayemntMethod = 1,
                State = "Riyadh",
                ZipCode = "6575",
                UserName = "yahia"
            } };
            return orders;
        }
    }
}
