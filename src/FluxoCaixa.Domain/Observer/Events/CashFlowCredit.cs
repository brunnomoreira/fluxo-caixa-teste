using FluxoCaixa.Domain.CashFlows;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluxoCaixa.Domain.Observer.Events
{
    public class CashFlowCredit : DomainEvent
    {
        public Credit credit { get; private set; }

        public CashFlowCredit(Credit credit)
        {
            this.credit = credit;
        }
    }
}
