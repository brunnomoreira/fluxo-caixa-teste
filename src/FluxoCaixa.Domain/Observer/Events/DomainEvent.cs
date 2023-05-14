using System;
using System.Collections.Generic;
using System.Text;

namespace FluxoCaixa.Domain.Observer.Events
{
    public class DomainEvent : IEvent
    {
        public Guid EventId { get; private set; }
        public DateTime PublishDateTime { get; private set; }
        public DomainEvent()
        {
            this.EventId = Guid.NewGuid();
            this.PublishDateTime = DateTime.Now;
        }
    }
}
