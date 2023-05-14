using FluxoCaixa.Domain.Observer.Events;
using FluxoCaixa.Domain.Observer.Handlers;
using System;

namespace FluxoCaixa.Domain.Observer.Manager
{
    public interface IEventManager
    {
        public void Publish(IEvent @event);

        public void Subscribe(Type type, IHandler handler);
    }
}
