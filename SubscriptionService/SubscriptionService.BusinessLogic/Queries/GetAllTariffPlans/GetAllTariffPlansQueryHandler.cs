using AutoMapper;
using MediatR;
using SubscriptionService.BusinessLogic.Extensions;
using SubscriptionService.BusinessLogic.Models;
using SubscriptionService.BusinessLogic.Models.TariffPlan;
using SubscriptionService.DataAccess.Entities;
using SubscriptionService.DataAccess.Repositories.Interfaces;

namespace SubscriptionService.BusinessLogic.Queries.GetAllTariffPlans
{
    public class GetAllTariffPlansQueryHandler
        : IRequestHandler<GetAllTariffPlansQuery, PageResponse<GetTariffPlanDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllTariffPlansQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PageResponse<GetTariffPlanDto>> Handle(GetAllTariffPlansQuery request, CancellationToken cancellationToken)
        {
            var tariffPlans = await _unitOfWork.TariffPlans.GetAllAsync(
                request.PageRequest.CurrentPage,
                request.PageRequest.PageSize,
                cancellationToken);

            var allPlansCount = await _unitOfWork.TariffPlans.CountAsync(cancellationToken);

            return tariffPlans.GetPageResponse<TariffPlan, GetTariffPlanDto>(allPlansCount, request.PageRequest, _mapper);
        }
    }
}
