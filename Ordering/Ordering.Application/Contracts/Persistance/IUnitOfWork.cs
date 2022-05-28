using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Contracts.Persistance
{
    public interface IUnitOfWork:IDisposable
    {
        public IOrderRepository Orders { get; }
        public Task<bool> SaveChangesAsync();
    }
}
