namespace FluxoCaixa.Application.Commands.Register
{
    using System.Threading.Tasks;    
    using FluxoCaixa.Application.Repositories;
    using FluxoCaixa.Domain.CashFlows;
    using FluxoCaixa.Domain.Observer.Events;
    using FluxoCaixa.Domain.Observer.Manager;

    public sealed class RegisterUseCase : IRegisterUseCase    {

        private readonly IEventManager eventManager;
        private readonly ICashFlowWriteOnlyRepository cashFlowWriteOnlyRepository;

        public RegisterUseCase(
            IEventManager eventManager,
            ICashFlowWriteOnlyRepository cashFlowWriteOnlyRepository
        )
        {
            this.eventManager = eventManager;
            this.cashFlowWriteOnlyRepository = cashFlowWriteOnlyRepository;
        }

        public async Task<RegisterResult> Execute(int year, double initialAmount) {
            CashFlow cashFlow = new CashFlow(year);
            cashFlow.Credit(initialAmount);
            Credit credit = (Credit)cashFlow.GetLastEntry();            

            
            await cashFlowWriteOnlyRepository.Add(cashFlow, credit);

            RegisterResult result = new RegisterResult(cashFlow);

            eventManager.Publish(new CashFlowCredit(credit));

            return result;
        }
    }
}
