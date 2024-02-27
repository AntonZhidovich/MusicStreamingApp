using AutoMapper;
using MusicService.Application.Models;

namespace MusicService.Infrastructure.Extensions
{
    public static class GetPageResponseExtension
    {
        public static PageResponse<TDto> GetPageResponse<TSource, TDto>(
            this IEnumerable<TSource> source, 
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
