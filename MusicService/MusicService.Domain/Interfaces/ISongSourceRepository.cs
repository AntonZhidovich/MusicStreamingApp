namespace MusicService.Domain.Interfaces
{
    public interface ISongSourceRepository
    {
        Task UploadAsync(string prefix, string sourceName, Stream sourceStream, CancellationToken cancellationToken = default);
        Task RemoveAsync(string prefix, string sourceName, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> GetFromPrefixAsync(string  prefix, CancellationToken cancellationToken = default);
        Task<bool> SourceExistsAsync(string fullSourceName);
        
        Task<MemoryStream> GetSourceStream(string prefix, 
            string sourceName, 
            long offset = 0,
            long length = 0,
            CancellationToken cancellationToken = default);
    }
}
