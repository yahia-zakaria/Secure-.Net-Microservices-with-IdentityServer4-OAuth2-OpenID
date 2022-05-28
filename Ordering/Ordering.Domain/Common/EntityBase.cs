using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Common
{
    public abstract class EntityBase
    {
        public int Id { get; protected set; }
        public int CreatedBy { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public int ModifiedBy { get; protected set; }
        public DateTime ModifiedAt { get; protected set; }
    }
}
