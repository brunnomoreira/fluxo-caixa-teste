using FluxoCaixa.Domain.Observer.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace FluxoCaixa.Domain.Observer.Handlers
{
    public class CashFlowCreditHandler : IHandler
    {
        public async Task Handle(IEvent @event)
        {
            Debug.WriteLine($"New Credit Event Received: " + JsonConvert.SerializeObject(@event));
            await Task.CompletedTask;
        }
    }
}
