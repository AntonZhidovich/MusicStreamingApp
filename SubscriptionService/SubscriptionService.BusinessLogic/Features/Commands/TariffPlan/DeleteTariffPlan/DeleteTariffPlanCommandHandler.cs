﻿using AutoMapper;
using MediatR;
using SubscriptionService.BusinessLogic.Constants;
using SubscriptionService.BusinessLogic.Exceptions;
using SubscriptionService.DataAccess.Repositories.Interfaces;

namespace SubscriptionService.BusinessLogic.Features.Commands.DeleteTariffPlan
{
    public class DeleteTariffPlanCommandHandler
        : IRequestHandler<DeleteTariffPlanCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTariffPlanCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        }
    }
}
