using MusicService.Domain.Entities;
using MusicService.Domain.Interfaces;
using System.Linq.Expressions;

namespace MusicService.Application.Specifications
{
    public class ReleasesFromAuthorSpecification : ISpecification<Release>
    {
        public Expression<Func<Release, bool>> Criteria { get; }

        public Expression<Func<Release, object>>? OrderBy { get; } = null;

        public Expression<Func<Release, object>>? OrderByDescending { get; }

        public List<string> Includes { get; }

        public ReleasesFromAuthorSpecification(string author)
        {
            OrderBy = release => release.Name;
            Release release = new Release();

            Includes = new List<string>
            {
                $"{nameof(release.Authors)}",
                $"{nameof(release.Songs)}"
            };

            Criteria = release => release.Authors
            .Select(author => author.Name)
            .Contains(author);
        }

        public bool IsSatisfiedBy(Release release)
        {
            return Criteria.Compile()(release);
        }
    }
}
