using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using MusicService.Domain.Exceptions;
using MusicService.Domain.Interfaces;
using MusicService.Infrastructure.Options;
using System.Reactive.Linq;

namespace MusicService.Infrastructure.Repositories
{
    public class SongSourceRepository : ISongSourceRepository
    {
        private readonly IMinioClient _minioClient;
        private readonly MinioOptions _options;

        public SongSourceRepository(IMinioClient minioClient, IOptions<MinioOptions> options)
        {
            _minioClient = minioClient;
            _options = options.Value;
        }

        public async Task RemoveAsync(string prefix, string sourceName, CancellationToken cancellationToken = default)
        {
            var args = new RemoveObjectArgs()
                .WithBucket(_options.BucketName)
                .WithObject($"{Normalize(prefix)}/{Normalize(sourceName)}");
            
            await _minioClient.RemoveObjectAsync(args, cancellationToken);
        }

        public async Task UploadAsync(string prefix, string sourceName, Stream sourceStream, CancellationToken cancellationToken = default)
        {
            var args = new PutObjectArgs()
                .WithBucket(_options.BucketName)
                .WithObject($"{Normalize(prefix)}/{Normalize(sourceName)}")
                .WithStreamData(sourceStream)
                .WithObjectSize(sourceStream.Length)
                .WithContentType(_options.ContentType);

            await _minioClient.PutObjectAsync(args, cancellationToken);
        }

        public async Task<IEnumerable<string>> GetFromPrefixAsync(string prefix, CancellationToken cancellationToken = default)
        {
            var args = new ListObjectsArgs()
                .WithBucket(_options.BucketName)
                .WithPrefix(Normalize(prefix))
                .WithRecursive(true);

            List<string> sources = new List<string>();
            var observable = _minioClient.ListObjectsAsync(args, cancellationToken);
            
            observable.Subscribe(
                item => sources.Add(item.Key),
                exception => throw new BadRequestException("Error while loading song sources list."));
            
            await observable;

            return sources;
        }

        public async Task<bool> SourceExistsAsync(string fullSourceName)
        {
            var args = new StatObjectArgs()
                .WithBucket(_options.BucketName)
                .WithObject(fullSourceName);

            await _minioClient.StatObjectAsync(args);

            return true;
        }

        public async Task<Stream> GetSourceStream(string prefix, 
            string sourceName,
            Stream outputStream,
            long offset = 0,
            long length = 0,
            CancellationToken cancellationToken = default)
        {
            var args = new GetObjectArgs()
                .WithBucket(_options.BucketName)
                .WithObject($"{Normalize(prefix)}/{Normalize(sourceName)}")
                .WithCallbackStream(stream => stream.CopyTo(outputStream));

            if (length > 0 || offset > 0)
            {
                args.WithOffsetAndLength(offset, length);
                outputStream.Position = offset;
            }

            await _minioClient.GetObjectAsync(args, cancellationToken);

            outputStream.Position = offset;

            return outputStream;
        }

        private string Normalize(string source)
        {
            return source.Trim().ToLower();
        }
    }
}
