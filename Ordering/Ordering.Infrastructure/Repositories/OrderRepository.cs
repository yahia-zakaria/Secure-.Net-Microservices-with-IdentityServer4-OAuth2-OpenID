using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistance;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        private readonly OrderContext context;
        public OrderRepository(OrderContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<List<Order>> GetOrderByUserNameAsync(string username)
        {
            return await context.Orders.Where(order => order.UserName == username).ToListAsync();  
        }
    }
}
