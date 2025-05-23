using FlyEasy.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FlyEasy.Data
{
    public class FlightContext
    {
        private readonly IMongoDatabase _database;

        public FlightContext(IOptions<MongoDbSettings> mongoDbSettings)
        {
            try
            {
                var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
                _database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd połączenia z MongoDB: " + ex.Message);
            }
        }

        public IMongoCollection<Flight> Flights => _database.GetCollection<Flight>("flights");
    }

    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
    }
}