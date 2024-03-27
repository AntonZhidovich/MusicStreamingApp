using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SubscriptionService.BusinessLogic.Constants;
using SubscriptionService.BusinessLogic.Exceptions;
using SubscriptionService.BusinessLogic.Models.TariffPlan;
using SubscriptionService.DataAccess.Repositories.Interfaces;

namespace SubscriptionService.BusinessLogic.Features.Commands.DeleteTariffPlan
{
    public class DeleteTariffPlanCommandHandler
        : IRequestHandler<DeleteTariffPlanCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheRepository _cache;
        private readonly ILogger<DeleteTariffPlanCommandHandler> _logger;

        public DeleteTariffPlanCommandHandler(IUnitOfWork unitOfWork, ICacheRepository cache, ILogger<DeleteTariffPlanCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
            _logger = logger;
        }

        public async Task Handle(DeleteTariffPlanCommand request, CancellationToken cancellationToken)
        {
            var tariffPlan = await _unitOfWork.TariffPlans.GetByIdAsync(request.Id, cancellationToken);

            if (tariffPlan == null)
            {
                _logger.LogError("Tariff plan with id {Id} not found.", request.Id);

                throw new NotFoundException(ExceptionMessages.tariffPlanNotFound);
            }

            _unitOfWork.TariffPlans.Delete(tariffPlan);

            await _unitOfWork.CommitAsync(cancellationToken);

            await _cache.RemoveAsync($"{typeof(GetTariffPlanDto)}{tariffPlan.Name.Trim().ToLower()}", cancellationToken);
        }
    }
}
