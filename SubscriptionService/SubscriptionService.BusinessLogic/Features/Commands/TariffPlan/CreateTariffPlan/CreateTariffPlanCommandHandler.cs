using AutoMapper;
using MediatR;
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

        public CreateTariffPlanCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetTariffPlanDto> Handle(CreateTariffPlanCommand request, CancellationToken cancellationToken)
        {
            var tariffPlan = _mapper.Map<TariffPlan>(request.Dto);
            tariffPlan.Id = Guid.NewGuid().ToString();

            await _unitOfWork.TariffPlans.CreateAsync(tariffPlan, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return _mapper.Map<GetTariffPlanDto>(tariffPlan);
        }
    }
}
