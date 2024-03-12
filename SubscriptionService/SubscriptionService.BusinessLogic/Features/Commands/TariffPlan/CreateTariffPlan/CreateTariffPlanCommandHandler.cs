using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SubscriptionService.BusinessLogic.Models.TariffPlan;
using SubscriptionService.DataAccess.Entities;
using SubscriptionService.DataAccess.Repositories.Interfaces;

namespace SubscriptionService.BusinessLogic.Features.Commands.CreateTariffPlan
{
    public class CreateTariffPlanCommandHandler
        : IRequestHandler<CreateTariffPlanCommand, GetTariffPlanDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateTariffPlanCommandHandler> _logger;

        public CreateTariffPlanCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateTariffPlanCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetTariffPlanDto> Handle(CreateTariffPlanCommand request, CancellationToken cancellationToken)
        {
            var tariffPlan = _mapper.Map<TariffPlan>(request.Dto);
            tariffPlan.Id = Guid.NewGuid().ToString();

            await _unitOfWork.TariffPlans.CreateAsync(tariffPlan, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            _logger.LogInformation("Tariff plan {Name} with id {Id} is persisted.", tariffPlan.Name, tariffPlan.Id);

            return _mapper.Map<GetTariffPlanDto>(tariffPlan);
        }
    }
}
