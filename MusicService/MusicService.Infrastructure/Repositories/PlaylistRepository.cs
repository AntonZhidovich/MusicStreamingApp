using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MusicService.Application.Options;
using MusicService.Domain.Entities;
using MusicService.Domain.Interfaces;

namespace MusicService.Infrastructure.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly IMongoCollection<Playlist> _playlistCollection;

        public PlaylistRepository(IOptions<PlaylistDbOptions>  dbOptions, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(dbOptions.Value.DatabaseName);
            _playlistCollection = database.GetCollection<Playlist>(dbOptions.Value.PlaylistsCollectionName);
        }

        public async Task CreateAsync(Playlist playlist, CancellationToken cancellationToken = default)
        {
            await _playlistCollection.InsertOneAsync(playlist,new InsertOneOptions(), cancellationToken);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Playlist>.Filter.Eq(playlist => playlist.Id, id);
            await _playlistCollection.DeleteOneAsync(filter, cancellationToken);
        }

        public async Task<Playlist?> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            var cursor = await _playlistCollection.FindAsync(filter: playlist => playlist.Id == id, cancellationToken: cancellationToken);

            return cursor?.First();
        }

        public async Task<IEnumerable<Playlist>> GetUserPlaylistsAsync(string userName, CancellationToken cancellationToken = default)
        {
            var cursor = await _playlistCollection.FindAsync(filter: playlist => playlist.UserName == userName, cancellationToken: cancellationToken);

            return await cursor.ToListAsync();
        }

        public async Task UpdateAsync(Playlist playlist, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Playlist>.Filter.Eq(playlist => playlist.Id, playlist.Id);
            await _playlistCollection.ReplaceOneAsync(filter: filter, replacement: playlist, cancellationToken: cancellationToken);
        }
    }
}
