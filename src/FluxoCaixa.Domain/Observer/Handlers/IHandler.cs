using FluxoCaixa.Domain.Observer.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluxoCaixa.Domain.Observer.Handlers
{
    public interface IHandler
    {
        public Task Handle(IEvent @event);
    }
}
