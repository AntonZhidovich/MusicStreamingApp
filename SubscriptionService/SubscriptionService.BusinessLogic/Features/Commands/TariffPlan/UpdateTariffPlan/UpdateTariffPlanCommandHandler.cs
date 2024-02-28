using AutoMapper;
using MediatR;
using SubscriptionService.BusinessLogic.Constants;
using SubscriptionService.BusinessLogic.Exceptions;
using SubscriptionService.BusinessLogic.Models.TariffPlan;
using SubscriptionService.DataAccess.Repositories.Interfaces;

namespace SubscriptionService.BusinessLogic.Features.Commands.UpdateTariffPlan
{
    public class UpdateTariffPlanCommandHandler
        : IRequestHandler<UpdateTariffPlanCommand, GetTariffPlanDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateTariffPlanCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetTariffPlanDto> Handle(UpdateTariffPlanCommand request, CancellationToken cancellationToken)
        {
            var tariffPlan = await _unitOfWork.TariffPlans.GetByIdAsync(request.Id, cancellationToken);

            if (tariffPlan == null)
            {
                throw new NotFoundException(ExceptionMessages.tariffPlanNotFound);
            }

            _mapper.Map(request.Dto, tariffPlan);
            _unitOfWork.TariffPlans.Update(tariffPlan);

            await _unitOfWork.CommitAsync(cancellationToken);

            return _mapper.Map<GetTariffPlanDto>(tariffPlan);
        }
    }
}
