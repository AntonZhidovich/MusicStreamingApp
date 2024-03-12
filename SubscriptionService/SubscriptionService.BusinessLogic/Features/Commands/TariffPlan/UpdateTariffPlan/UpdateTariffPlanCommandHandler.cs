using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ICacheRepository _cache;
        private readonly ILogger<UpdateTariffPlanCommandHandler> _logger;

        public UpdateTariffPlanCommandHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ICacheRepository cache,
            ILogger<UpdateTariffPlanCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
        }

        public async Task<GetTariffPlanDto> Handle(UpdateTariffPlanCommand request, CancellationToken cancellationToken)
        {
            var tariffPlan = await _unitOfWork.TariffPlans.GetByIdAsync(request.Id, cancellationToken);

            if (tariffPlan == null)
            {
                _logger.LogError("Tariff plan with id {Id} not found.", request.Id);

                throw new NotFoundException(ExceptionMessages.tariffPlanNotFound);
            }

            _mapper.Map(request.Dto, tariffPlan);
            _unitOfWork.TariffPlans.Update(tariffPlan);

            await _unitOfWork.CommitAsync(cancellationToken);

            await _cache.RemoveAsync($"{typeof(GetTariffPlanDto)}{tariffPlan.Name.Trim().ToLower()}", cancellationToken);

            return _mapper.Map<GetTariffPlanDto>(tariffPlan);
        }
    }
}
