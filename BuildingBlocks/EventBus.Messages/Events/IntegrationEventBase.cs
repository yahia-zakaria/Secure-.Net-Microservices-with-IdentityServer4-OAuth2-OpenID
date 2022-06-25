using System;
namespace EventBus.Messages.Events
{
    public class IntegrationEventBase
    {
        public IntegrationEventBase()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.Now;
        }
        public IntegrationEventBase(Guid id, DateTime creationDate)
        {
            Id = id;
            CreationDate = creationDate;
        }

        public Guid Id { get; private set; }
        public DateTime CreationDate { get; private set; }
    }
}
