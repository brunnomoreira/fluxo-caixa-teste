using FluxoCaixa.Domain.CashFlows;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluxoCaixa.Domain.Observer.Events
{
    public class CashFlowDebit : DomainEvent
    {
        public Debit debit { get; private set; }

        public CashFlowDebit(Debit debit)
        {
            this.debit = debit;
        }
    }
}
