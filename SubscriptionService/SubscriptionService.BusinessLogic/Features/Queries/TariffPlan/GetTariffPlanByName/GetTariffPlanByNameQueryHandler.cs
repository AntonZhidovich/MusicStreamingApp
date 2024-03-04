using AutoMapper;
using MediatR;
using SubscriptionService.BusinessLogic.Constants;
using SubscriptionService.BusinessLogic.Exceptions;
using SubscriptionService.BusinessLogic.Models.TariffPlan;
using SubscriptionService.DataAccess.Repositories.Interfaces;

namespace SubscriptionService.BusinessLogic.Features.Queries.GetTariffPlanByName
{
    public class GetTariffPlanByNameQueryHandler :
        IRequestHandler<GetTariffPlanByNameQuery, GetTariffPlanDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheRepository _cacheRepository;

        public GetTariffPlanByNameQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheRepository cacheRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheRepository = cacheRepository;
        }

        public async Task<GetTariffPlanDto> Handle(GetTariffPlanByNameQuery request, CancellationToken cancellationToken)
        {
            string key = $"{typeof(GetTariffPlanDto)}{request.Name.Trim().ToLower()}";

            var tariffPlanDto = await _cacheRepository.GetAsync<GetTariffPlanDto>(key, cancellationToken);

            if (tariffPlanDto != null)
            {
                return tariffPlanDto;
            }

            var tariffPlan = await _unitOfWork.TariffPlans.GetByNameAsync(request.Name, cancellationToken);

            if (tariffPlan == null)
            {
                throw new NotFoundException(ExceptionMessages.tariffPlanNotFound);
            }

            tariffPlanDto = _mapper.Map<GetTariffPlanDto>(tariffPlan);

            await _cacheRepository.SetAsync(key, tariffPlanDto, cancellationToken);

            return tariffPlanDto;
        }
    }
}
