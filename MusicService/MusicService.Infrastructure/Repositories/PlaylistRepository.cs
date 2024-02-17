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
        private readonly IMongoCollection<UserPlaylistTariff> _userTariffCollection;

        public PlaylistRepository(IOptions<MongoDbOptions> dbOptions, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(dbOptions.Value.DatabaseName);
            _playlistCollection = database.GetCollection<Playlist>(dbOptions.Value.PlaylistsCollectionName);
            _userTariffCollection = database.GetCollection<UserPlaylistTariff>(dbOptions.Value.UserTariffCollectionName);
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

            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Playlist>> GetUserPlaylistsAsync(string userName, 
            int? maxCount = null, 
            CancellationToken cancellationToken = default)
        {
            var cursor = _playlistCollection.Find(filter: playlist => playlist.UserName == userName)
                .SortBy(playlist => playlist.CreatedAt)
                .Limit(maxCount);

            return await cursor.ToListAsync();
        }

        public async Task<int> CountAsync(string userName, CancellationToken cancellationToken = default)
        {
            var longCount = await _playlistCollection.CountDocumentsAsync(playlist => playlist.UserName == userName,
                cancellationToken: cancellationToken);

            return (int)longCount;
        }

        public async Task UpdateAsync(Playlist playlist, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Playlist>.Filter.Eq(playlist => playlist.Id, playlist.Id);
            
            await _playlistCollection.ReplaceOneAsync(filter: filter, replacement: playlist, cancellationToken: cancellationToken);
        }

        public async Task<int?> GetUserMaxPlaylistCountAsync(string userName, CancellationToken cancellationToken = default)
        {
            var cursor = await _userTariffCollection.FindAsync(filter: tariff => tariff.UserName == userName,
                cancellationToken: cancellationToken);
            
            var tariff = cursor?.FirstOrDefault();

            return tariff?.MaxPlaylistCount;
        }

        public async Task DeleteUserPlaylistsAsync(string userName, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Playlist>.Filter.Eq(tariff => tariff.UserName, userName);
            
            await _playlistCollection.DeleteManyAsync(filter, cancellationToken);
        }

        public async Task UpsertUserPlaylistTariffAsync(UserPlaylistTariff tariff, CancellationToken cancellationToken = default)
        {
            var options = new ReplaceOptions { IsUpsert = true };
            var filter = Builders<UserPlaylistTariff>.Filter.Eq(tariff => tariff.UserName, tariff.UserName);
            
            await _userTariffCollection.ReplaceOneAsync(filter, tariff, options, cancellationToken);
        }

        public async Task DeleteUserPlaylistTariffAsync(string userName, CancellationToken cancellationToken = default)
        {
            var filter = Builders<UserPlaylistTariff>.Filter.Eq(tariff => tariff.UserName, userName);
            
            await _userTariffCollection.DeleteOneAsync(filter, cancellationToken);
        }
    }
}
