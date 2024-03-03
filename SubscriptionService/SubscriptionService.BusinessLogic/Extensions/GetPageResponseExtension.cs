using AutoMapper;
using SubscriptionService.BusinessLogic.Models;

namespace SubscriptionService.BusinessLogic.Extensions
{
    internal static class GetPageResponseExtension
    {
        public static PageResponse<TDto> GetPageResponse<TEntity, TDto>(
            this IEnumerable<TEntity> source, 
            int entityCount, 
            GetPageRequest request,
            IMapper mapper)
        {
            var pagesCount = (int)Math.Ceiling((double)entityCount / request.PageSize);
            var pageResponse = new PageResponse<TDto>
            {
                PagesCount = pagesCount,
                CurrentPage = request.CurrentPage,
                PageSize = request.PageSize,
                Items = mapper.Map<IEnumerable<TDto>>(source)
            };

            return pageResponse;
        }
    }
}
