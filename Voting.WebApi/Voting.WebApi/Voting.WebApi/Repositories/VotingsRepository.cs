using VotingWebApi.Entities;
using MongoDB.Driver;

namespace VotingWebApi.Repositories
{
    public class VotingsRepository
    {
        private const string dbName = "VotingsDb";
        private const string collectionName = "votings";
        private readonly IMongoCollection<Voting> dbCollection;
        private readonly FilterDefinitionBuilder<Voting> filterBuilder = Builders<Voting>.Filter;

        public VotingsRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase(dbName);
            dbCollection = database.GetCollection<Voting>(collectionName);
        }

        public async Task<IReadOnlyCollection<Voting>> GetAll()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<IReadOnlyCollection<Voting>> GetAllEnabledVotings()
        {
            FilterDefinition<Voting> filter = filterBuilder.Eq(voting => voting.IsEnabled, true);

            return await dbCollection.Find(filter).ToListAsync();
        }

        public async Task<Voting> GetById(int votingId)
        {
            FilterDefinition<Voting> filter = filterBuilder.Eq(voting => voting.Id, votingId);

            return await dbCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<bool> ExistsWithId(int votingId)
        {
            FilterDefinition<Voting> filter = filterBuilder.Eq(voting => voting.Id, votingId);

            return await dbCollection.Find(filter).AnyAsync();
        }

        public async Task Create(Voting voting)
        {
            if (voting == null)
            {
                throw new ArgumentNullException(nameof(voting));
            }

            await dbCollection.InsertOneAsync(voting);
        }

        public async Task Update(Voting voting)
        {
            if (voting == null)
            {
                throw new ArgumentNullException(nameof(voting));
            }

            FilterDefinition<Voting> filter = filterBuilder.Eq(existingVoting => existingVoting.Id, voting.Id);

            await dbCollection.ReplaceOneAsync(filter, voting);
        }
    }
}
