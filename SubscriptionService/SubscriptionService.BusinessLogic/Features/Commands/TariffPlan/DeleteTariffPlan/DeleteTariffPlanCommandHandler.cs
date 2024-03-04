using AutoMapper;
using MediatR;
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

        public DeleteTariffPlanCommandHandler(IUnitOfWork unitOfWork, ICacheRepository cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task Handle(DeleteTariffPlanCommand request, CancellationToken cancellationToken)
        {
            var tariffPlan = await _unitOfWork.TariffPlans.GetByIdAsync(request.Id, cancellationToken);

            if (tariffPlan == null)
            {
                throw new NotFoundException(ExceptionMessages.tariffPlanNotFound);
            }

            _unitOfWork.TariffPlans.Delete(tariffPlan);

            await _unitOfWork.CommitAsync(cancellationToken);

            await _cache.RemoveAsync($"{typeof(GetTariffPlanDto)}{tariffPlan.Name.Trim().ToLower()}", cancellationToken);
        }
    }
}
